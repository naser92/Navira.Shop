using Navira.Shop.Domain.Catalog;

namespace Navira.Shop.Application.Catalog
{
    public class ProductAttributeValuePolicy : IProductAttributeValuePolicy
    {
        private const decimal MaxNumber = 99_999_999_999_999.9999m;
        public AttributeValueValidationResult Validate(ProductAttribute definition, ProductAttributeValueResponseDto input, ProductAttributeValue current, IReadOnlyDictionary<int, ProductAttributeOption> optionsById)
        {
            ArgumentNullException.ThrowIfNull(definition);
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(optionsById);

            AttributeValueValidationResult Fail(string message)
                => AttributeValueValidationResult.Fail(
                    $"ویژگی «{definition.Name}»: {message}");

            if (definition.Id != input.ProductAttributeId)
                return Fail("شناسه ویژگی با تعریف آن مطابقت ندارد.");

            if (definition.IsDeleted)
                return Fail("حذف شده است.");

            if (definition.IsVariantAttribute)
                return Fail("فقط در بخش تنوع‌های محصول قابل استفاده است.");

            if (!Enum.IsDefined(
                    typeof(AttributeValueType),
                    definition.ValueType))
            {
                return Fail("نوع مقدار معتبر نیست.");
            }

            if (current is not null &&
                (current.IsDeleted ||
                 current.ProductAttributeId != definition.Id))
            {
                return Fail("رکورد فعلی برای این ویرایش معتبر نیست.");
            }

            var type = (AttributeValueType)definition.ValueType;

            var optionIds = input.ProductAttributeOptionIds
                ?? new List<int>();

            // فقط NULL به معنی ارسال‌نشدن مقدار مستقیم است.
            // بنابراین false و 0 مقدار محسوب می‌شوند.
            var directValueCount =
                (input.ProductAttributeOptionId.HasValue ? 1 : 0) +
                (input.TextValue is not null ? 1 : 0) +
                (input.NumberValue.HasValue ? 1 : 0) +
                (input.BooleanValue.HasValue ? 1 : 0) +
                (input.DateValue.HasValue ? 1 : 0);

            if (type == AttributeValueType.MultiOption)
            {
                if (directValueCount != 0)
                    return Fail(
                        "برای نوع چندانتخابی نباید مقدار مستقیم ارسال شود.");

                if (optionIds.Count == 0)
                    return Fail("حداقل یک گزینه انتخاب کنید.");

                if (optionIds.Any(id => id <= 0))
                    return Fail("شناسه گزینه معتبر نیست.");

                if (optionIds.Distinct().Count() != optionIds.Count)
                    return Fail("گزینه تکراری ارسال شده است.");
            }
            else
            {
                if (optionIds.Count != 0)
                    return Fail(
                        "لیست گزینه‌ها فقط برای نوع چندانتخابی مجاز است.");

                if (directValueCount != 1)
                    return Fail("دقیقاً یک مقدار متناسب با نوع ویژگی ارسال کنید.");
            }

            switch (type)
            {
                case AttributeValueType.Option:
                    if (!input.ProductAttributeOptionId.HasValue ||
                        input.ProductAttributeOptionId.Value <= 0)
                    {
                        return Fail("یک گزینه معتبر انتخاب کنید.");
                    }

                    break;

                case AttributeValueType.MultiOption:
                    // ساختار مقدار بالاتر بررسی شده است.
                    break;

                case AttributeValueType.Text:
                    if (string.IsNullOrWhiteSpace(input.TextValue))
                        return Fail("مقدار متنی نمی‌تواند خالی باشد.");

                    if (input.TextValue.Length > 2000)
                        return Fail("متن حداکثر ۲۰۰۰ کاراکتر است.");

                    break;

                case AttributeValueType.Number:
                case AttributeValueType.Decimal:
                    if (!input.NumberValue.HasValue)
                        return Fail("مقدار عددی الزامی است.");

                    var number = input.NumberValue.Value;

                    if (number < -MaxNumber || number > MaxNumber)
                        return Fail("عدد خارج از محدوده قابل ذخیره است.");

                    if (type == AttributeValueType.Number &&
                        decimal.Truncate(number) != number)
                    {
                        return Fail("عدد باید صحیح باشد.");
                    }

                    // مقدار را گرد نمی‌کنیم؛ مقدار ناسازگار رد می‌شود.
                    if (decimal.Round(number, 4) != number)
                        return Fail("حداکثر چهار رقم اعشار مجاز است.");

                    break;

                case AttributeValueType.Boolean:
                    if (!input.BooleanValue.HasValue)
                        return Fail("مقدار بله یا خیر را مشخص کنید.");

                    break;

                case AttributeValueType.Date:
                    if (!input.DateValue.HasValue)
                        return Fail("تاریخ الزامی است.");

                    break;

                default:
                    return Fail("نوع مقدار پشتیبانی نمی‌شود.");
            }

            // ویژگی غیرفعال فقط بدون تغییر قابل نگهداری است.
            if (!definition.IsActive)
            {
                if (current is null)
                    return Fail("غیرفعال است و نمی‌توان آن را اضافه کرد.");

                if (!HasSameValue(type, input, current))
                    return Fail(
                        "غیرفعال است؛ فقط حفظ مقدار قبلی یا حذف آن مجاز است.");
            }

            // گزینه‌های قبلی باید از دیتابیس بارگذاری شده باشند.
            var previousOptionIds = GetCurrentOptionIds(type, current);

            var requestedOptionIds = type switch
            {
                AttributeValueType.Option =>
                    new[] { input.ProductAttributeOptionId!.Value },

                AttributeValueType.MultiOption =>
                    optionIds.ToArray(),

                _ => Array.Empty<int>()
            };

            foreach (var optionId in requestedOptionIds)
            {
                if (!optionsById.TryGetValue(optionId, out var option) ||
                    option.IsDeleted)
                {
                    return Fail($"گزینه با شناسه {optionId} وجود ندارد.");
                }

                if (option.ProductAttributeId != definition.Id)
                    return Fail(
                        $"گزینه با شناسه {optionId} متعلق به این ویژگی نیست.");

                if (!option.IsActive &&
                    !previousOptionIds.Contains(optionId))
                {
                    return Fail(
                        $"گزینه «{option.Value}» غیرفعال است و قابل انتخاب نیست.");
                }
            }

            return AttributeValueValidationResult.Success();
        }

        private static HashSet<int> GetCurrentOptionIds(
            AttributeValueType type,
            ProductAttributeValue? current)
        {
            if (current is null)
                return new HashSet<int>();

            if (type == AttributeValueType.Option)
            {
                return current.ProductAttributeOptionId.HasValue
                    ? new HashSet<int>
                    {
                    current.ProductAttributeOptionId.Value
                    }
                    : new HashSet<int>();
            }

            if (type == AttributeValueType.MultiOption)
            {
                return current.ProductAttributeValueOption
                    .Where(x => !x.IsDeleted)
                    .Select(x => x.ProductAttributeOptionId)
                    .ToHashSet();
            }

            return new HashSet<int>();
        }

        private static bool HasSameValue(
            AttributeValueType type,
            ProductAttributeValueResponseDto input,
            ProductAttributeValue current)
        {
            return type switch
            {
                AttributeValueType.Option =>
                    input.ProductAttributeOptionId ==
                    current.ProductAttributeOptionId,

                AttributeValueType.MultiOption =>
                    GetCurrentOptionIds(type, current)
                        .SetEquals(
                            input.ProductAttributeOptionIds
                            ?? new List<int>()),

                AttributeValueType.Text =>
                    string.Equals(
                        input.TextValue,
                        current.TextValue,
                        StringComparison.Ordinal),

                AttributeValueType.Number or AttributeValueType.Decimal =>
                    input.NumberValue == current.NumberValue,

                AttributeValueType.Boolean =>
                    input.BooleanValue == current.BooleanValue,

                AttributeValueType.Date =>
                    input.DateValue == current.DateValue,

                _ => false
            };
        }

    }
}
