using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Navira.Shop.Application.Media;
using System.Text.RegularExpressions;

namespace Navira.Shop.Infrastructure.Media
{
    public class MinIOFileService : IMinIOFileService, IDisposable
    {
        private readonly IMinioClient _minio;
        private readonly MinIOSettings _settings;
        private readonly SemaphoreSlim _bucketLock = new(1, 1);
        private bool _bucketEnsured;

        public MinIOFileService(IMinioClient minio, IOptions<MinIOSettings> options)
        {
            _minio = minio;
            _settings = options.Value;
            if (string.IsNullOrWhiteSpace(_settings.BucketName))
            {
                throw new InvalidOperationException(
                    "MinIO BucketName is not configured.");
            }
        }

        public async Task<StoredImage> SaveAsync(Guid mediaFileId, PreparedImage image, CancellationToken cancellationToken = default)
        {
            if (mediaFileId == Guid.Empty)
                throw new ArgumentException(
                    "MediaFileId cannot be empty.",
                    nameof(mediaFileId));

            ArgumentNullException.ThrowIfNull(image);
            ArgumentNullException.ThrowIfNull(image.Metadata);
            ArgumentNullException.ThrowIfNull(image.Variants);

            // همه ورودی‌ها پیش از اولین آپلود بررسی می‌شوند.
            ValidateStream(
                image.Original,
                image.Metadata.SizeBytes);

            ValidateImageFormat(
                image.OriginalExtension,
                image.Metadata.ContentType);

            if (image.Metadata.Width <= 0 || image.Metadata.Height <= 0)
                throw new ArgumentException("Invalid original dimensions.");

            if (image.Variants.Count == 0)
                throw new ArgumentException("No prepared image variants.");

            var variantNames = new HashSet<string>(
                StringComparer.Ordinal);

            foreach (var variant in image.Variants)
            {
                ArgumentNullException.ThrowIfNull(variant);

                ValidateProfile(variant.Profile);
                ValidateImageFormat(variant.Extension, variant.ContentType);
                ValidateStream(variant.Content, variant.SizeBytes);

                if (variant.Width <= 0 || variant.Height <= 0)
                    throw new ArgumentException("Invalid variant dimensions.");

                var name = variant.Profile
                    + variant.Extension.ToLowerInvariant();

                if (!variantNames.Add(name))
                {
                    throw new ArgumentException(
                        $"Duplicate image variant: {name}");
                }
            }

            await EnsureBucketAsync(cancellationToken);

            // هر اجرای ذخیره، مسیر مستقل دارد تا فایل منتشرشده بازنویسی نشود.
            var uploadId = Guid.NewGuid();

            var prefix = $"media/{mediaFileId:N}/{uploadId:N}";

            var originalKey =
                $"{prefix}/original/source" +
                image.OriginalExtension.ToLowerInvariant();

            var original = await UploadAsync(
                originalKey,
                image.Original,
                image.Metadata.ContentType,
                image.Metadata.SizeBytes,
                cancellationToken);

            var storedVariants = new List<StoredImageVariant>(
                image.Variants.Count);

            // ترتیبی: مصرف منابع و مدیریت Stream ساده‌تر است.
            foreach (var variant in image.Variants)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var objectKey =
                    $"{prefix}/variants/{variant.Profile}" +
                    variant.Extension.ToLowerInvariant();

                var storedFile = await UploadAsync(
                    objectKey,
                    variant.Content,
                    variant.ContentType,
                    variant.SizeBytes,
                    cancellationToken);

                storedVariants.Add(new StoredImageVariant(
                    Profile: variant.Profile,
                    Width: variant.Width,
                    Height: variant.Height,
                    File: storedFile));
            }

            // تنها پس از موفقیت تمام آپلودها نتیجه برمی‌گردد.
            return new StoredImage(
                Original: original,
                Variants: storedVariants);
        }

        private async Task<StoredObject> UploadAsync(string objectKey, Stream content, string contentType, long sizeBytes, CancellationToken cancellationToken)
        {
            // PreparedImage خروجی Streamهای مستقل و Seekable دارد.
            content.Position = 0;

            var args = new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectKey)
                .WithStreamData(content)
                .WithObjectSize(sizeBytes)
                .WithContentType(contentType);

            await _minio.PutObjectAsync(
                args,
                cancellationToken);

            // مالک Stream همچنان PreparedImage است.
            return new StoredObject(
                Bucket: _settings.BucketName,
                ObjectKey: objectKey,
                ContentType: contentType,
                SizeBytes: sizeBytes);
        }

        private async Task EnsureBucketAsync(CancellationToken cancellationToken)
        {
            await _bucketLock.WaitAsync(cancellationToken);

            try
            {
                if (_bucketEnsured)
                    return;

                var exists = await BucketExistsAsync(
                    cancellationToken);

                if (!exists)
                {
                    try
                    {
                        await _minio.MakeBucketAsync(
                            new MakeBucketArgs()
                                .WithBucket(_settings.BucketName),
                            cancellationToken);
                    }
                    catch (MinioException)
                    {
                        // ممکن است instance دیگری هم‌زمان Bucket را ساخته باشد.
                        // فقط در صورت تأیید وجود Bucket ادامه می‌دهیم.
                        if (!await BucketExistsAsync(cancellationToken))
                            throw;
                    }
                }

                _bucketEnsured = true;
            }
            finally
            {
                _bucketLock.Release();
            }
        }

        private Task<bool> BucketExistsAsync(CancellationToken cancellationToken) =>
            _minio.BucketExistsAsync(new BucketExistsArgs().WithBucket(_settings.BucketName), cancellationToken);

        private static void ValidateStream(Stream content, long expectedSize)
        {
            ArgumentNullException.ThrowIfNull(content);

            if (!content.CanRead || !content.CanSeek)
            {
                throw new ArgumentException(
                    "Prepared streams must be readable and seekable.");
            }

            if (expectedSize <= 0 || content.Length != expectedSize)
            {
                throw new ArgumentException(
                    "Stream length does not match the declared size.");
            }
        }

        private static void ValidateProfile(string profile)
        {
            if (string.IsNullOrWhiteSpace(profile) ||
                profile.Length > 64 ||
                !Regex.IsMatch(profile, @"\A[a-z0-9]+(?:-[a-z0-9]+)*\z"))
            {
                throw new ArgumentException(
                    "Invalid variant profile.");
            }
        }

        private static void ValidateImageFormat(string extension, string contentType)
        {
            var expectedContentType =
                extension?.ToLowerInvariant() switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".webp" => "image/webp",
                    _ => null
                };

            if (expectedContentType is null ||
                !string.Equals(
                    expectedContentType,
                    contentType,
                    StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Image extension and content type are invalid.");
            }
        }

        public void Dispose()
        {
            _bucketLock.Dispose();
        }



    }


}
