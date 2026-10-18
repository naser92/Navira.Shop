using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Navira.Shop.Domain.Catalog;

public sealed record Slug
{
    public const int MaxLength = 350;
    public const int MinLength = 2;

    private static readonly Regex PrefixPattern = new(
        @"^nav[0-9]{10}_",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant,
        TimeSpan.FromSeconds(1));

    private static readonly Regex ValidPattern = new(
        @"^(?:NAV[0-9]{10}_)?[\p{L}0-9]+(?:-[\p{L}0-9]+)*$",
        RegexOptions.CultureInvariant,
        TimeSpan.FromSeconds(1));

    public string Value { get; }

    private Slug(string value)
    {
        Value = value;
    }

    // برای Slug واردشده توسط کاربر یا مقدار خوانده‌شده از دیتابیس
    public static Slug Create(string value)
    {
        var normalized = Normalize(value);

        Validate(normalized);

        return new Slug(normalized);
    }

    // برای تولید خودکار از نام محصول
    public static Slug Generate(string productName)
    {
        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentException(
                "نام محصول برای تولید Slug الزامی است.",
                nameof(productName));
        }

        var namePart = NormalizeText(productName);

        if (string.IsNullOrEmpty(namePart))
            namePart = "product";

        var prefix = $"NAV{GenerateRandomNumber()}_";

        // فقط در تولید خودکار، نام را متناسب با فضای باقی‌مانده کوتاه می‌کنیم.
        namePart = Truncate(namePart, MaxLength - prefix.Length);

        var value = prefix + namePart;

        Validate(value);

        return new Slug(value);
    }

    public static string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Slug نمی‌تواند خالی باشد.",
                nameof(value));
        }

        var prepared = Prepare(value);

        // حفظ پیشوند تولیدشده هنگام خواندن یا ویرایش مجدد
        var prefixMatch = PrefixPattern.Match(prepared);

        if (prefixMatch.Success)
        {
            var prefix = prefixMatch.Value.ToUpperInvariant();
            var namePart = NormalizeText(
                prepared[prefixMatch.Length..]);

            return prefix + namePart;
        }

        return NormalizeText(prepared);
    }

    // مقدار canonical را اعتبارسنجی می‌کند؛ تغییری در آن نمی‌دهد.
    public static void Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Slug نمی‌تواند خالی باشد.",
                nameof(value));
        }

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            throw new ArgumentException(
                $"طول Slug باید بین {MinLength} و {MaxLength} کاراکتر باشد.",
                nameof(value));
        }

        if (!ValidPattern.IsMatch(value))
        {
            throw new ArgumentException(
                "ساختار Slug معتبر نیست.",
                nameof(value));
        }

        if (!string.Equals(
                value,
                Normalize(value),
                StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "Slug باید نرمال‌شده باشد.",
                nameof(value));
        }
    }

    private static string Prepare(string value)
    {
        var builder = new StringBuilder(
            value.Trim().Normalize(NormalizationForm.FormKC));

        for (var i = 0; i < builder.Length; i++)
        {
            var character = builder[i];

            builder[i] = character switch
            {
                'ي' or 'ى' => 'ی',
                'ك' => 'ک',

                >= '۰' and <= '۹'
                    => (char)('0' + character - '۰'),

                >= '٠' and <= '٩'
                    => (char)('0' + character - '٠'),

                _ => character
            };
        }

        return builder.ToString().ToLowerInvariant();
    }

    private static string NormalizeText(string value)
    {
        var prepared = Prepare(value);
        var builder = new StringBuilder(prepared.Length);

        var pendingSeparator = false;

        foreach (var character in prepared)
        {
            var category = char.GetUnicodeCategory(character);

            // حذف اعراب و کشیده
            if (category is UnicodeCategory.NonSpacingMark
                or UnicodeCategory.SpacingCombiningMark
                or UnicodeCategory.EnclosingMark
                || character == '\u0640')
            {
                continue;
            }

            // حروف فارسی/لاتین و ارقام انگلیسی
            if (char.IsLetter(character)
                || character is >= '0' and <= '9')
            {
                if (pendingSeparator && builder.Length > 0)
                    builder.Append('-');

                builder.Append(character);
                pendingSeparator = false;
            }
            else
            {
                // فاصله، نیم‌فاصله، underscore و علائم → یک خط تیره
                pendingSeparator = builder.Length > 0;
            }
        }

        return builder.ToString();
    }

    private static string GenerateRandomNumber()
    {
        // رقم اول صفر نیست؛ طول عدد همیشه ۱۰ رقم است.
        Span<char> digits = stackalloc char[10];

        digits[0] = (char)(
            '0' + RandomNumberGenerator.GetInt32(1, 10));

        for (var i = 1; i < digits.Length; i++)
        {
            digits[i] = (char)(
                '0' + RandomNumberGenerator.GetInt32(0, 10));
        }

        return new string(digits);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (value.Length <= maxLength)
            return value;

        return value[..maxLength].TrimEnd('-');
    }

    public override string ToString() => Value;
}
