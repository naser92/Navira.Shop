using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Media;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace Navira.Shop.Application.Media
{
    public class ImagePreparationService : IImagePreparationService
    {
        private const long MaxFileBytes = 10 * 1024 * 1024;
        private const long MaxPixels = 25_000_000;
        private const int MaxDimension = 16_384;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task<PreparedImage> PrepareAsync(Stream source, MediaUploadProfile profile, ImageEditDto edit, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (!source.CanRead)
                throw new ResultException("فایل قابل خواندن نیست.");

            ValidateEdit(edit);

            var settings = GetProfile(profile);

            var original = new MemoryStream();

            var variants = new List<PreparedImageVariant>();
            var ownershipTransferred = false;

            try
            {
                await CopyBoundedAsync(
                    source,
                    original,
                    cancellationToken);

                if (original.Length == 0)
                    throw new ResultException("Media.EmptyFile", "فایل تصویر خالی است.");

                original.Position = 0;

                var format = await Image.DetectFormatAsync(
                    original,
                    cancellationToken);

                var (contentType, extension) =
                    format.Name.ToUpperInvariant() switch
                    {
                        "JPEG" => ("image/jpeg", ".jpg"),
                        "PNG" => ("image/png", ".png"),
                        "WEBP" => ("image/webp", ".webp"),

                        _ => throw new ResultException(
                            "Media.UnsupportedFormat",
                            "فقط تصاویر JPEG، PNG و WebP مجاز هستند.")
                    };

                // قبل از Decode کامل، ابعاد بررسی می‌شوند.
                original.Position = 0;

                var info = await Image.IdentifyAsync(
                    original,
                    cancellationToken);

                ValidateDimensions(info.Width, info.Height);

                original.Position = 0;

                var decoderOptions = new DecoderOptions
                {
                    // حداکثر دو فریم برای تشخیص و رد تصویر متحرک.
                    MaxFrames = 2,

                    // EXIF برای اصلاح جهت لازم است.
                    SkipMetadata = false,

                    ColorProfileHandling = ColorProfileHandling.Convert
                };

                using var image = await Image.LoadAsync<Rgba32>(
                    decoderOptions,
                    original,
                    cancellationToken);

                if (image.Frames.Count > 1)
                {
                    throw new ResultException(
                        "Media.AnimationNotAllowed",
                        "تصویر متحرک مجاز نیست.");
                }

                ValidateDimensions(image.Width, image.Height);

                cancellationToken.ThrowIfCancellationRequested();

                // مختصات ورودی باید نسبت به تصویر با جهت اصلاح‌شده باشند.
                image.Mutate(context => context.AutoOrient());

                var originalWidth = image.Width;
                var originalHeight = image.Height;

                var cropRectangle = GetCropRectangle(
                    image.Width,
                    image.Height,
                    edit.Crop);

                image.Mutate(context => context.Crop(cropRectangle));

                var background = Color.ParseHex(edit.BackgroundColor);

                foreach (var target in settings.Targets)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var variant = await CreateVariantAsync(
                        image,
                        target,
                        edit.Placement,
                        background,
                        cancellationToken);

                    variants.Add(variant);
                }

                original.Position = 0;

                var result = new PreparedImage
                {
                    Original = original,
                    OriginalExtension = extension,

                    Metadata = new PreparedImageMetadata(
                        ContentType: contentType,
                        Width: originalWidth,
                        Height: originalHeight,
                        SizeBytes: original.Length,
                        ProcessingProfile: settings.Name,
                        ProcessingProfileVersion: 1,
                        EditSettingsJson: JsonSerializer.Serialize(
                            edit,
                            JsonOptions)),

                    Variants = variants
                };

                ownershipTransferred = true;

                return result;
            }
            catch (UnknownImageFormatException)
            {
                throw new ResultException(
                    "Media.InvalidFormat",
                    "محتوای فایل، تصویر قابل پشتیبانی نیست.");
            }
            catch (InvalidImageContentException)
            {
                throw new ResultException(
                    "Media.CorruptedImage",
                    "فایل تصویر خراب یا ناقص است.");
            }
            finally
            {
                // در موفقیت، PreparedImage مالک Streamها می‌شود.
                if (!ownershipTransferred)
                {
                    foreach (var variant in variants)
                        await variant.Content.DisposeAsync();

                    await original.DisposeAsync();
                }
            }

        }

        private static async Task<PreparedImageVariant> CreateVariantAsync(Image<Rgba32> cropped, TargetProfile target, ImagePlacementDto placement, Color background,
       CancellationToken cancellationToken)
        {
            // ضریب جاگیری تصویر در بوم هدف با حفظ نسبت.
            var fit = Math.Min(
                (double)target.Width / cropped.Width,
                (double)target.Height / cropped.Height);

            var requestedResize = fit * placement.Scale;

            // اگر رزولوشن کافی نیست، کل بوم کوچک می‌شود.
            // به این ترتیب نسبت حاشیه و جای‌گیری حفظ می‌شود.
            var canvasScale = Math.Min(1d, 1d / requestedResize);

            var canvasWidth = Math.Max(
                1,
                (int)Math.Floor(target.Width * canvasScale));

            var canvasHeight = Math.Max(
                1,
                (int)Math.Floor(target.Height * canvasScale));

            var actualFit = Math.Min(
                (double)canvasWidth / cropped.Width,
                (double)canvasHeight / cropped.Height);

            var resize = Math.Min(1d, actualFit * placement.Scale);

            var imageWidth = Math.Max(
                1,
                (int)Math.Floor(cropped.Width * resize));

            var imageHeight = Math.Max(
                1,
                (int)Math.Floor(cropped.Height * resize));

            var left = placement.CenterX * canvasWidth - imageWidth / 2d;
            var top = placement.CenterY * canvasHeight - imageHeight / 2d;

            const double tolerance = 0.000001;

            if (left < -tolerance ||
                top < -tolerance ||
                left + imageWidth > canvasWidth + tolerance ||
                top + imageHeight > canvasHeight + tolerance)
            {
                throw new ResultException(
                    "Media.PlacementOutsideCanvas",
                    "بخشی از تصویر خارج از کادر قرار می‌گیرد.");
            }

            var x = Math.Clamp(
                (int)Math.Round(left),
                0,
                canvasWidth - imageWidth);

            var y = Math.Clamp(
                (int)Math.Round(top),
                0,
                canvasHeight - imageHeight);

            cancellationToken.ThrowIfCancellationRequested();

            using var resized = cropped.Clone(context =>
                context.Resize(new ResizeOptions
                {
                    Size = new Size(imageWidth, imageHeight),
                    Mode = ResizeMode.Stretch,
                    Sampler = KnownResamplers.Lanczos3
                }));

            // نسبت ابعاد بالا محاسبه شده؛ Stretch فقط همان ابعاد را اعمال می‌کند.
            // بوم جدید، متادیتای فایل ورودی را به ارث نمی‌برد.
            using var canvas = new Image<Rgba32>(
                canvasWidth,
                canvasHeight,
                background.ToPixel<Rgba32>());

            canvas.Mutate(context =>
                context.DrawImage(resized, new Point(x, y), 1f));

            var output = new MemoryStream();

            try
            {
                await canvas.SaveAsync(
                    output,
                    new WebpEncoder
                    {
                        FileFormat = WebpFileFormatType.Lossy,
                        Quality = target.Quality,
                        SkipMetadata = true
                    },
                    cancellationToken);

                output.Position = 0;

                return new PreparedImageVariant(
                    Profile: target.Name,
                    ContentType: "image/webp",
                    Extension: ".webp",
                    Width: canvasWidth,
                    Height: canvasHeight,
                    SizeBytes: output.Length,
                    Content: output);
            }
            catch
            {
                await output.DisposeAsync();
                throw;
            }
        }

        private static Rectangle GetCropRectangle(int width, int height, NormalizedCropDto crop)
        {
            var x = (int)Math.Floor(crop.X * width);
            var y = (int)Math.Floor(crop.Y * height);

            var right = Math.Min(
                width,
                (int)Math.Ceiling((crop.X + crop.Width) * width));

            var bottom = Math.Min(
                height,
                (int)Math.Ceiling((crop.Y + crop.Height) * height));

            if (x >= width || y >= height || right <= x || bottom <= y)
            {
                throw new ResultException(
                    "Media.InvalidCrop",
                    "کادر انتخاب‌شده معتبر نیست.");
            }

            return new Rectangle(x, y, right - x, bottom - y);
        }

        private static void ValidateDimensions(int width, int height)
        {
            if (width <= 0 ||
                height <= 0 ||
                width > MaxDimension ||
                height > MaxDimension ||
                (long)width * height > MaxPixels)
            {
                throw new ResultException(
                    "Media.InvalidDimensions",
                    "ابعاد تصویر بیش از حد مجاز یا نامعتبر است.");
            }
        }

        private static async Task CopyBoundedAsync(Stream source, Stream destination, CancellationToken cancellationToken)
        {
            var buffer = new byte[81920];
            long total = 0;

            while (true)
            {
                var read = await source.ReadAsync(
                    buffer.AsMemory(),
                    cancellationToken);

                if (read == 0)
                    break;

                total += read;

                if (total > MaxFileBytes)
                {
                    throw new ResultException(
                        "Media.FileTooLarge",
                        "حجم تصویر نباید بیشتر از ۱۰ مگابایت باشد.");
                }

                await destination.WriteAsync(
                    buffer.AsMemory(0, read),
                    cancellationToken);
            }
        }

        private static ProcessingProfile GetProfile(
      MediaUploadProfile profile) =>
      profile switch
      {
          MediaUploadProfile.ProductImage => new(
              "product-image",
              new[]
              {
                    new TargetProfile("thumbnail", 160, 160, 80),
                    new TargetProfile("product-card", 480, 480, 80),
                    new TargetProfile("product-detail", 900, 900, 82)
              }),

          MediaUploadProfile.CategoryImage => new(
              "category-image",
              new[]
              {
                    new TargetProfile("category", 400, 400, 80)
              }),

          MediaUploadProfile.BannerDesktop => new(
              "banner-desktop",
              new[]
              {
                    new TargetProfile("banner-desktop", 1920, 640, 82)
              }),

          MediaUploadProfile.BannerMobile => new(
              "banner-mobile",
              new[]
              {
                    new TargetProfile("banner-mobile", 800, 1000, 82)
              }),

          _ => throw new ResultException(
              "Media.InvalidProfile",
              "نوع کاربرد تصویر معتبر نیست.")
      };


        private static bool InRange(double value, double min, double max) =>
                double.IsFinite(value) && value >= min && value <= max;

        private static void ValidateEdit(ImageEditDto edit)
        {
            if (edit?.Crop is null || edit.Placement is null)
            {
                throw new ResultException(
                    "Media.EditRequired",
                    "تنظیمات ویرایش الزامی است.");
            }

            var crop = edit.Crop;
            var placement = edit.Placement;

            if (!InRange(crop.X, 0, 1) ||
                !InRange(crop.Y, 0, 1) ||
                !InRange(crop.Width, 0.000001, 1) ||
                !InRange(crop.Height, 0.000001, 1) ||
                crop.X + crop.Width > 1 + 0.000000001 ||
                crop.Y + crop.Height > 1 + 0.000000001)
            {
                throw new ResultException(
                    "Media.InvalidCrop",
                    "مختصات برش معتبر نیست.");
            }

            if (!InRange(placement.CenterX, 0, 1) ||
                !InRange(placement.CenterY, 0, 1) ||
                !InRange(placement.Scale, 0.01, 1))
            {
                throw new ResultException(
                    "Media.InvalidPlacement",
                    "تنظیمات جای‌گیری تصویر معتبر نیست.");
            }

            if (edit.BackgroundColor is null ||
                !Regex.IsMatch(
                    edit.BackgroundColor,
                    @"\A#[0-9a-fA-F]{6}\z"))
            {
                throw new ResultException(
                    "Media.InvalidBackgroundColor",
                    "رنگ پس‌زمینه باید مانند #FFFFFF باشد.");
            }
        }
    }
}
