namespace AspNetScaffolding.Extensions.AutoMapper
{
    public interface ITypeAdapter {

        TTarget Adapt<TSource, TTarget>(TSource source) where TTarget : class where TSource : class;

        TTarget Adapt<TTarget>(object source) where TTarget : class;
    }
}