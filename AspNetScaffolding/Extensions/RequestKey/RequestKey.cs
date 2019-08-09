namespace AspNetScaffolding.Extensions.RequestKey
{
    public class RequestKey
    {
        public RequestKey() {}

        public RequestKey(string value)
        {
            this.Value = value;
        }

        public readonly string Value;
    }
}
