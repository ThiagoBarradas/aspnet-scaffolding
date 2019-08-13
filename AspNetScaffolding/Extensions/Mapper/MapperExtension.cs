using AutoMapper;

namespace AspNetScaffolding.Extensions.Mapper
{
    public static class GlobalMapper
    {
        public static IMapper Mapper { get; set; }

        public static TDestination Map<TDestination>(this object source) 
            where TDestination : class
        {
            return GlobalMapper.Mapper.Map<TDestination>(source);
        }

        public static TDestination Map<TSource, TDestination>(this TDestination destination, TSource source) 
            where TSource : class 
            where TDestination : class
        {
            return GlobalMapper.Mapper.Map(source, destination);
        }
    }
}
