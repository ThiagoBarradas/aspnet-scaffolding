using System.Collections.Generic;

namespace AspNetScaffolding.Extensions.AutoMapper
{
    public static class ProjectionsExtensions {

        public static TTarget As<TTarget>(this object source)
            where TTarget : class {

            var adapter = TypeAdapterFactory.Create();
            return adapter.Adapt<TTarget>(source);
        }

        public static List<TTarget> AsCollection<TTarget>(this IEnumerable<object> source)
            where TTarget : class {

            var adapter = TypeAdapterFactory.Create();
            return adapter.Adapt<List<TTarget>>(source);
        }
    }
}