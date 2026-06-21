using System.Globalization;

namespace Navira.Shop.Core.Extensions
{
    public static class ExtensionBase
    {
        public static bool IsNull(this object obj) => obj is null;
        public static bool IsNotNull(this object obj) => obj is not null;


        public static bool IsNumber(this string value) => Int64.TryParse(value, NumberStyles.AllowExponent, null, out _) || decimal.TryParse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint, null, out _);
        public static bool IsDigit(this string value) => value.IsMatch("^[0-9]+$");

        public static decimal ToDecimal(this string value) => value.IsNullOrWhiteSpace() ? 0 : decimal.Parse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint, null);

        #region int

        public static int ToInt(this string value, string defaultValue = null)
        {
            int result;
            int.TryParse(value ?? defaultValue, NumberStyles.AllowExponent, null, out result);
            return result;
        }

        public static bool IsPositive(this int? number)
        {
            return number != null && number > 0;
        }
        public static bool IsPositive(this int number)
        {
            return number > 0;
        }
        public static bool IsNegativeOrZero(this int number)
        {
            return number == 0 || number < 0;
        }

        public static bool IsNullOrNegativeOrZero(this int? number)
        {
            return number == null || number == 0 || number < 0;
        }

        #endregion

        #region long
        public static bool IsPositive(this long? number)
        {
            return number != null && number > 0;
        }
        public static bool IsPositive(this long number)
        {
            return number > 0;
        }
        public static bool IsNegativeOrZero(this long number)
        {
            return number == 0 || number < 0;
        }
        #endregion

        public static bool IsNullOrNegativeOrZero(this long? number)
        {
            return number == null || number == 0 || number < 0;
        }



        public static bool IsPositive(this float number)
        {
            return number > 0;
        }
        public static bool IsNegativeOrZero(this decimal number)
        {
            return number == 0 || number < 0;
        }
        public static bool IsNegative(this decimal number)
        {
            return number < 0;
        }
        public static bool IsPositive(this decimal number)
        {
            return number > 0;
        }
        public static bool IsNullOrNegativeOrZero(this decimal? number)
        {
            return number == null || number == 0 || number < 0;
        }
        public static bool IsNullOrZero(this decimal? number)
        {
            return number == null || number == 0;
        }
        public static bool IsNegativeOrZero(this float number)
        {
            return number == 0 || number < 0;
        }
        public static decimal ToNegative(this decimal number)
        {
            return number > 0 ? number * -1 : number;
        }
        public static decimal ToPositive(this decimal number)
        {
            return number < 0 ? number * -1 : number;
        }
        public static decimal? ToNegative(this decimal? number)
        {
            if (number == null)
                return null;
            return number > 0 ? number * -1 : number;
        }
        public static decimal? ToPositive(this decimal? number)
        {
            if (number == null)
                return null;
            return number < 0 ? number * -1 : number;
        }
        public static bool IsEmptyOrZero(this string value) => value.IsNullOrWhiteSpace() || value == "0";
        public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

        public static bool IsNullOrEmpty<T>(this ICollection<T> @this) => @this.IsNull() || !@this.Any();
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> @this) => @this.IsNull() || !@this.Any();
        public static bool IsNullOrEmpty<T>(this List<T> @this) => @this.IsNull() || !@this.Any();
        public static bool IsNullOrEmpty(this List<object> obj) => obj.IsNull() || !obj.Any();
        public static bool IsNullOrEmpty<T>(this T[] obj) => obj.IsNull() || !obj.Any();
        public static bool IsNullOrEmpty(this Guid? value) => value.IsNull() || IsEmpty(value.Value);
        public static bool IsNullOrEmpty(this DateTime? value) => value.IsNull() || IsEmpty(value.Value);


        public static bool IsNotNullOrWhiteSpace(this string value) => !string.IsNullOrWhiteSpace(value);
        public static bool IsNotNullOrEmpty<T>(this ICollection<T> @this) => !@this.IsNull() && @this.Any();
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> @this) => !@this.IsNull() && @this.Any();
        public static bool IsNotNullOrEmpty<T>(this List<T> @this) => !@this.IsNull() && @this.Any();
        public static bool IsNotNullOrEmpty(this List<object> obj) => !obj.IsNull() && obj.Any();
        public static bool IsNotNullOrEmpty<T>(this T[] obj) => !obj.IsNull() && obj.Any();
        public static bool IsNotNullOrEmpty(this Guid? value) => !value.IsNull() && !IsEmpty(value.Value);
        public static bool IsNotNullOrEmpty(this DateTime? value) => !value.IsNull() && !IsEmpty(value.Value);

        public static bool IsEmpty(this Guid value) => value == Guid.Empty;
        public static bool IsEmpty(this DateTime value) => value == DateTime.MinValue;


        public static long TryLongParse(this string value) => Int64.TryParse(value, out var result) ? result : 0;
        public static Guid? TryParseGuid(this string value) => value.IsNullOrWhiteSpace() ? null : Guid.TryParse(value, out var result) ? result : null;
        public static bool EnumContainsValue<TEnum>(this int? value) where TEnum : struct, IConvertible
        {
            return !(value is null) &&
                   value.Value.IsExistValueInEnum<TEnum>();
        }

        public static bool EnumContainsValue<TEnum>(this int value) where TEnum : struct, IConvertible
        {
            return value.IsExistValueInEnum<TEnum>();
        }
    }
}
