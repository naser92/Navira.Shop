using Navira.Shop.Application.Media;
using Navira.Shop.Domain.Media;
using System.Text.Json;

namespace Navira.Shop.Infrastructure.Media
{
    public class MediaPersistenceService : IMediaPersistenceService
    {
        private readonly IMediaFileWriteRepository _mediaFileWriteRepository;
        private readonly IMediaVariantQueryRepository _mediaVariantQueryRepository;
        private readonly IMediaVariantWriteRepository _mediaVariantWriteRepository;

        public MediaPersistenceService(IMediaFileWriteRepository mediaFileWriteRepository, IMediaVariantQueryRepository mediaVariantQueryRepository, IMediaVariantWriteRepository mediaVariantWriteRepository)
        {
            _mediaFileWriteRepository = mediaFileWriteRepository;
            _mediaVariantQueryRepository = mediaVariantQueryRepository;
            _mediaVariantWriteRepository = mediaVariantWriteRepository;
        }

        public async Task CompleteAsync(Guid mediaFileId, StoredImage image, CancellationToken cancellationToken)
        {
            if (mediaFileId == Guid.Empty)
                throw new ArgumentException("شناسه فایل معتبر نیست.");

            ArgumentNullException.ThrowIfNull(image);
            ArgumentNullException.ThrowIfNull(image.Original);
            ArgumentNullException.ThrowIfNull(image.Variants);

            ValidateStoredObject(image.Original);

            if (image.Variants.Count == 0)
                throw new ArgumentException("نسخه‌های تصویر ارسال نشده‌اند.");

            var profiles = new HashSet<string>(StringComparer.Ordinal);

            foreach (var variant in image.Variants)
            {
                ArgumentNullException.ThrowIfNull(variant);
                ArgumentNullException.ThrowIfNull(variant.File);

                ValidateStoredObject(variant.File);

                if (string.IsNullOrWhiteSpace(variant.Profile) ||
                    variant.Width <= 0 ||
                    variant.Height <= 0 ||
                    !string.Equals(
                        variant.File.ContentType,
                        "image/webp",
                        StringComparison.OrdinalIgnoreCase))
                {
                    throw new ArgumentException("مشخصات نسخه تصویر معتبر نیست.");
                }

                // قرارداد فعلی: یک WebP برای هر Profile.
                if (!profiles.Add(variant.Profile))
                    throw new ArgumentException("پروفایل خروجی تکراری است.");
            }




            var file = await _mediaFileWriteRepository.Get(mediaFileId);

            if (file is null)
                throw new InvalidOperationException("رکورد فایل پیدا نشد.");

            // این نسخه برای جریان هم‌زمان فعلی نوشته شده است.
            if (file.Status != (int)MediaStatus.Uploading)
            {
                throw new InvalidOperationException(
                    "وضعیت فایل برای تکمیل ثبت مجاز نیست.");
            }



            if (file.SizeBytes != image.Original.SizeBytes || !string.Equals(file.ContentType, image.Original.ContentType, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("مشخصات فایل ذخیره‌شده با فایل اولیه مطابقت ندارد.");

            if (await _mediaVariantQueryRepository.Any(x => x.MediaFileId == mediaFileId))
                throw new InvalidOperationException("برای این فایل قبلاً نسخه ثبت شده است.");

            var now = DateTime.UtcNow;

            var variants = image.Variants
                .Select(variant => new MediaVariant
                {
                    // فرض: Id این Entity از نوع bigint Identity است.
                    MediaFileId = mediaFileId,
                    Profile = variant.Profile,
                    Format = "webp",

                    Bucket = variant.File.Bucket,
                    ObjectKey = variant.File.ObjectKey,
                    ContentType = variant.File.ContentType,

                    Width = variant.Width,
                    Height = variant.Height,
                    SizeBytes = variant.File.SizeBytes,


                })
                .ToList();

            await _mediaVariantWriteRepository.Insert(variants);

            file.Bucket = image.Original.Bucket;
            file.ObjectKey = image.Original.ObjectKey;

            file.Status = (int)MediaStatus.Ready;


            // مهلت استفاده از زمان آماده‌شدن محاسبه می‌شود.
            file.ExpiresAt = now.AddHours(24);

        }

        private static void ValidateStoredObject(StoredObject file)
        {
            if (string.IsNullOrWhiteSpace(file.Bucket) ||
                string.IsNullOrWhiteSpace(file.ObjectKey) ||
                string.IsNullOrWhiteSpace(file.ContentType) ||
                file.SizeBytes <= 0)
            {
                throw new ArgumentException("مشخصات فایل ذخیره‌شده معتبر نیست.");
            }
        }

        public async Task CreateUploadingAsync(Guid mediaFileId, string originalFileName, PreparedImageMetadata metadata, CancellationToken cancellationToken)
        {
            if (mediaFileId == Guid.Empty)
                throw new ArgumentException("شناسه فایل معتبر نیست.");

            ArgumentNullException.ThrowIfNull(metadata);


            if (string.IsNullOrWhiteSpace(originalFileName) || originalFileName.Length > 255)
                throw new ArgumentException("نام فایل معتبر نیست.");

            if (metadata.SizeBytes <= 0 ||
                   metadata.Width <= 0 ||
                   metadata.Height <= 0 ||
                   metadata.ProcessingProfileVersion <= 0 ||
                   string.IsNullOrWhiteSpace(metadata.ContentType) ||
                   string.IsNullOrWhiteSpace(metadata.ProcessingProfile))
            {
                throw new ArgumentException("مشخصات تصویر معتبر نیست.");
            }

            var now = DateTime.UtcNow;

            // تنظیمات معتبر باید قبلاً در PrepareAsync تولید شده باشند.
            // اینجا حداقل ساختار JSON نیز کنترل می‌شود.
            using var settings = JsonDocument.Parse(metadata.EditSettingsJson);

            if (settings.RootElement.ValueKind != JsonValueKind.Object)
                throw new ArgumentException("تنظیمات ویرایش باید یک JSON Object باشد.");

            var file = new MediaFile
            {
                Id = mediaFileId,

                OriginalFileName = originalFileName,
                ContentType = metadata.ContentType,
                SizeBytes = metadata.SizeBytes,
                Width = metadata.Width,
                Height = metadata.Height,

                ProcessingProfile = metadata.ProcessingProfile,
                ProcessingProfileVersion = metadata.ProcessingProfileVersion,
                EditSettingsJson = metadata.EditSettingsJson,

                StorageProvider = "MinIO",

                // مسیر واقعی پس از موفقیت Storage مشخص می‌شود.
                Bucket = null,
                ObjectKey = null,

                Status = (int)MediaStatus.Uploading,

                // مهلت اولیه فایل بدون ارتباط با محصول یا بنر.
                ExpiresAt = now.AddHours(24),

            };

            await _mediaFileWriteRepository.Insert(file);
        }
    }
}
