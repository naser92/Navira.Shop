using Mapster;
namespace Navira.Shop.Core.Mapper
{
    public static class MapperExtention
    {
        public static TTarget Map<TSource, TTarget>(this TSource source)
        {
            return source.Adapt<TTarget>();
        }
        public static TTarget Map<TTarget>(this object source)
        {
            return source.Adapt<TTarget>();
        }
        public static TTarget Map<TSource, TTarget>(this TSource source, TTarget target)
        {
            return source.Adapt(target);
        }

        public static TTarget Map<TTarget>(this object source, TTarget target)
        {
            return source.Adapt(target);
        }
        public static TTarget Map<TSource, TTarget>(this TSource source, TypeAdapterConfig config)
        {
            return source.Adapt<TTarget>(config);
        }
        public static TTarget Map<TTarget>(this object source, TypeAdapterConfig config)
        {
            return source.Adapt<TTarget>(config);
        }
        public static TTarget Map<TSource, TTarget>(this TSource source, TTarget target, TypeAdapterConfig config)
        {
            return source.Adapt(target, config);
        }
        public static TTarget Map<TTarget>(this object source, TTarget target, TypeAdapterConfig config)
        {
            return source.Adapt(target, config);
        }



        public static object? Map(this object source, Type sourceType, Type destinationType)
        {
            return source.Adapt(sourceType, destinationType);
        }

        public static object? Map(
            this object source,
            Type sourceType,
            Type destinationType,
            TypeAdapterConfig config)
        {
            return source.Adapt(sourceType, destinationType, config);
        }

        public static object? Map(
            this object source,
            object destination,
            Type sourceType,
            Type destinationType)
        {

            return source.Adapt(destination, sourceType, destinationType);
        }

        public static object? Map(
            this object source,
            object destination,
            Type sourceType,
            Type destinationType,
            TypeAdapterConfig config)
        {

            return source.Adapt(destination, sourceType, destinationType, config);
        }


        public static IQueryable<T> ProjectTo<T>(this IQueryable query, TypeAdapterConfig config = null)
        {
            return query.ProjectToType<T>(config);
        }


    }
}
