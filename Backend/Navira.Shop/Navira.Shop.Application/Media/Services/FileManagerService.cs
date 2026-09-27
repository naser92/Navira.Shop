using Navira.Shop.Core.Results;
using Navira.Shop.Domain.Media;

namespace Navira.Shop.Application.Media
{
    public class FileManagerService : IFileManagerService
    {
        private const long MaxFileSizeBytes = 10 * 1024 * 1024;

        private readonly IImagePreparationService _imagePreparation;
        private readonly IMediaStorageService _storage;
        private readonly IMediaPersistenceService _persistence;

        public FileManagerService(
       IImagePreparationService imagePreparation,
       IMediaStorageService storage,
       IMediaPersistenceService persistence
       )
        {
            _imagePreparation = imagePreparation;
            _storage = storage;
            _persistence = persistence;

        }
        public async Task<IResult> SaveImage(UploadMediaRequest request, CancellationToken cancellationToken = default)
        {
            if (request?.File is null || request.File.Length == 0)
                return await Result.FailAsync("لطفاً  تصویر را انتخاب کنید");

            if (request.File.Length > MaxFileSizeBytes)
                return await Result.FailAsync("حجم تصویر نباید بیشتر از ۱۰ مگابایت باشد");


            if (request.Profile is null || !Enum.IsDefined(typeof(MediaUploadProfile), request.Profile.Value))
                return await Result.FailAsync("نوع کاربرد تصویر معتبر نیست");


            if (request.Edit is null || request.Edit.Crop is null || request.Edit.Placement is null)
                return await Result.FailAsync("تنظیمات ویرایش تصویر الزامی است");

            var mediaFileId = Guid.NewGuid();

            try
            {
                // بررسی واقعی فایل و تنظیمات ویرایش و تولید خروجی‌ها.
                // ContentType و پسوند ارسالی کاربر قابل اعتماد نیستند.
                await using var source = request.File.OpenReadStream();

                await using var prepared =
                    await _imagePreparation.PrepareAsync(
                        source,
                        request.Profile.Value,
                        request.Edit,
                        cancellationToken);

                // ثبت رکورد Uploading قبل از نوشتن در Storage.
                // این عملیات باید واقعاً در دیتابیس Commit شود.
                await _persistence.CreateUploadingAsync(
                    mediaFileId,
                    GetDisplayFileName(request.File.FileName),
                    prepared.Metadata,
                    cancellationToken);

                // ذخیره اصل خصوصی و همه نسخه‌های پردازش‌شده.
                // مسیرها توسط سرور و بر اساس mediaFileId ساخته می‌شوند.
                var stored = await _storage.SaveAsync(
                    mediaFileId,
                    prepared,
                    cancellationToken);

                // ثبت MediaVariantها و تغییر وضعیت به Ready
                // در یک تراکنش دیتابیس و Commit پیش از بازگشت.
                await _persistence.CompleteAsync(
                    mediaFileId,
                    stored,
                    cancellationToken);

                return await Result.SuccessAsync("فایل با موفقیت ذخیره شد");
            }
            catch (MediaValidationException exception)
            {

                return await Result.FailAsync($"Media Exception Code: {exception.Code}  Message: {exception.Message}");

            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                //_logger.LogInformation(
                //    "Image upload was cancelled. MediaFileId: {MediaFileId}",
                //    mediaFileId);

                return await Result.FailAsync($"Image upload was cancelled. MediaFileId: {mediaFileId}");

            }
            catch (Exception exception)
            {
                //_logger.LogError(
                //    exception,
                //    "Image registration failed. MediaFileId: {MediaFileId}",
                //    mediaFileId);

                return await Result.FailAsync("ثبت تصویر کامل نشد؛ لطفاً دوباره تلاش کنید.");
            }

        }

        private static string GetDisplayFileName(string fileName)
        {
            // نام اصلی صرفاً برای نمایش است؛ مسیر Storage از آن ساخته نمی‌شود.
            var normalized = (fileName ?? string.Empty).Replace('\\', '/');
            var name = normalized[(normalized.LastIndexOf('/') + 1)..];

            if (string.IsNullOrWhiteSpace(name))
                return "image";

            return name.Length <= 255 ? name : name[..255];
        }
    }
}
