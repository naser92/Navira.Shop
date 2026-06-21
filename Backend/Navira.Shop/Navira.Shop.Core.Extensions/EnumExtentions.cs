
using Navira.Shop.Core.Mapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Navira.Shop.Core.Extensions
{
    public static class EnumExtentions
    {
        public static string GetEnumDisplayName(Enum val)
        {
            return val.GetType().GetMember(val.ToString()).FirstOrDefault()?.GetCustomAttribute<DisplayAttribute>(false)?.Name ?? val.ToString();
        }
        public static string ToDescription<TEnum>(this TEnum val)
        {
            if (val == null)
            {
                return "";
            }

            var enumName = val.ToString();

            try
            {
                var field = val.GetType().GetField(enumName);

                var descriptionAttr = field.GetCustomAttribute<DescriptionAttribute>(false);

                if (descriptionAttr != null && !string.IsNullOrWhiteSpace(descriptionAttr.Description))
                    return descriptionAttr.Description;

                var displayAttr = field.GetCustomAttribute<DisplayAttribute>(false);

                if (displayAttr != null)
                {
                    var name = displayAttr.GetName();
                    if (!string.IsNullOrEmpty(name))
                    {
                        return name;
                    }
                }

                return enumName;
            }
            catch (Exception)
            {
                return enumName;
            }
        }

        public static string ToDescription<TEnum>(this TEnum val, string message)
        {
            return message + val.ToDescription();
        }
        public static List<TResult> GetList<T, TResult>(this T inputEnum) where T : Enum
        {

            var valuelist = Enum.GetValues(typeof(T)).Cast<T>().Select(a => new
            {
                Id = Convert.ToInt32(a),
                Code = Convert.ToInt32(a),
                TitleLatin = Convert.ToString(a),
                Title = GetEnumDescription<T>(Convert.ToString(a)),
                IsActive = true
            });

            return valuelist.Map<List<TResult>>();
        }

        private static string GetEnumDescription<T>(string enumValue)
        {
            var value = Enum.Parse(typeof(T), enumValue);
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }
        public static TEnum GetValue<TEnum>(this TEnum val, string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static int GetId<TEnum>(this TEnum currentKey) where TEnum : struct
        {
            return Convert.ToInt32(currentKey);
        }

        public static string GetStringId<TEnum>(this TEnum currentKey) where TEnum : struct
        {
            return Convert.ToInt32(currentKey).ToString();
        }

        public static bool IsExistValueInEnum<TEnum>(this TEnum? value) where TEnum : struct
        {

            return value.IsNotNull() && value.Value.IsExistValueInEnum();
        }
        public static bool IsNotExistValueInEnum<TEnum>(this TEnum? value) where TEnum : struct
        {

            return !value.IsExistValueInEnum();
        }
        public static bool IsExistValueInEnum<TEnum>(this TEnum value) where TEnum : struct
        {

            return Enum.IsDefined(typeof(TEnum), value);
        }
        public static bool IsNotExistValueInEnum<TEnum>(this TEnum value) where TEnum : struct
        {

            return !value.IsExistValueInEnum();
        }

        public static bool IsExistValueInEnum<T>(this int? value) where T : struct, IConvertible
        {

            return value.IsNotNull() && value.Value.IsExistValueInEnum<T>();
        }
        public static bool IsNotExistValueInEnum<T>(this int? value) where T : struct, IConvertible
        {

            return value.IsExistValueInEnum<T>();
        }
        public static bool IsExistValueInEnum<T>(this int value) where T : struct, IConvertible
        {

            return Enum.IsDefined(typeof(T), value);
        }
        public static bool IsNotExistValueInEnum<T>(this int value) where T : struct, IConvertible
        {

            return !value.IsExistValueInEnum();
        }
    }
}
