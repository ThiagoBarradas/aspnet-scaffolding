namespace AspNetScaffolding.Extensions.AutoMapper
{
    public static class TypeAdapterFactory {

        #region Members

        static volatile ITypeAdapterFactory _factory = null;

        #endregion

        #region Public Static Methods

        public static void Set(ITypeAdapterFactory factory) {
            _factory = factory;
        }

        public static ITypeAdapter Create() {
            return _factory.Create();
        }

        #endregion
    }
}