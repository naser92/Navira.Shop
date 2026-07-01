namespace Navira.Shop.Core.Extensions
{
    public static class TypeExtentions
    {
        public static bool IsNumericType(this Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }

        public static bool IsAssignableToGeneric(this Type type, Type genericType) => type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);

        public static Type GetInheritedGenericArgumentType(this Type type, Type genericType) => type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType)?.GetGenericArguments()[0];

    }
}
