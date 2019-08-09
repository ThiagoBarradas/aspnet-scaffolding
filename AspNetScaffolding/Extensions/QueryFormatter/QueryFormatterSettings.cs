using AspNetScaffolding.Extensions.JsonSerializer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PackUtils;
using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.QueryFormatter
{
    public static class QueryFormatterSettings
    {
        public static void AddQueryFormatter(this MvcOptions mvcOptions, JsonSerializerEnum jsonSerializer)
        {
            CaseQueryValueProvider.JsonSerializerMode = jsonSerializer;
            mvcOptions.ValueProviderFactories.Add(new CaseQueryValueProviderFactory());
        }
    }

    public class CaseQueryValueProvider : QueryStringValueProvider, IValueProvider
    {
        public static JsonSerializerEnum JsonSerializerMode { get; set; }

        public CaseQueryValueProvider(
            BindingSource bindingSource,
            IQueryCollection values,
            System.Globalization.CultureInfo culture)
            : base(bindingSource, values, culture)
        {
        }

        public override bool ContainsPrefix(string prefix)
        {
            return base.ContainsPrefix(GetNewValue(prefix));
        }

        public override ValueProviderResult GetValue(string key)
        {
            return base.GetValue(GetNewValue(key));
        }

        private string GetNewValue(string value)
        {
            switch (JsonSerializerMode)
            {
                case JsonSerializerEnum.Camelcase:
                    value = value.ToCamelCase();
                    break;
                case JsonSerializerEnum.Snakecase:
                    value = value.ToSnakeCase();
                    break;
                case JsonSerializerEnum.Lowercase:
                    value = value.ToLowerCase();
                    break;
            }

            return value;
        }
    }

    public class CaseQueryValueProviderFactory : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var valueProvider = new CaseQueryValueProvider(
                BindingSource.Query,
                context.ActionContext.HttpContext.Request.Query,
                System.Globalization.CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);

            return Task.CompletedTask;
        }
    }
}
