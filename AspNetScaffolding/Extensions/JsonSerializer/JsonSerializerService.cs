using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PackUtils;
using PackUtils.Converters;
using System;

namespace AspNetScaffolding.Extensions.JsonSerializer
{
    public static class JsonSerializerService
    {
        public static void ConfigureJsonSettings(
            this IMvcBuilder mvc, 
            IServiceCollection services,
            JsonSerializerEnum jsonSerializerMode, 
            string timezoneHeaderName,
            TimeZoneInfo defaultTimeZone)
        {
            CaseUtility.JsonSerializerMode = jsonSerializerMode;

            JsonSerializerSettings jsonSerializerSettings = null;
            Newtonsoft.Json.JsonSerializer jsonSerializer = null;

            switch (jsonSerializerMode)
            {
                case JsonSerializerEnum.Camelcase:
                    jsonSerializer = JsonUtility.CamelCaseJsonSerializer;
                    jsonSerializerSettings = JsonUtility.CamelCaseJsonSerializerSettings;
                    break;
                case JsonSerializerEnum.Lowercase:
                    jsonSerializer = JsonUtility.LowerCaseJsonSerializer;
                    jsonSerializerSettings = JsonUtility.LowerCaseJsonSerializerSettings;
                    break;
                case JsonSerializerEnum.Snakecase:
                    jsonSerializer = JsonUtility.SnakeCaseJsonSerializer;
                    jsonSerializerSettings = JsonUtility.SnakeCaseJsonSerializerSettings;
                    break;
                default:
                    break;
            }

            jsonSerializer.Converters.Clear();
            jsonSerializer.Converters.Add(new EnumWithContractJsonConverter());
            jsonSerializerSettings.Converters.Clear();
            jsonSerializerSettings.Converters.Add(new EnumWithContractJsonConverter());

            JsonConvert.DefaultSettings = () => jsonSerializerSettings;

            services.AddScoped((provider) => jsonSerializer);
            services.AddScoped((provider) => jsonSerializerSettings);

            DateTimeConverter.DefaultTimeZone = defaultTimeZone;
            mvc.AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = jsonSerializerSettings.ContractResolver;
                options.SerializerSettings.Converters = jsonSerializerSettings.Converters;
                options.SerializerSettings.NullValueHandling = jsonSerializerSettings.NullValueHandling;
                options.SerializerSettings.Converters.Add(new DateTimeConverter(() => {
                    var httpContextAccessor = services.BuildServiceProvider().GetService<IHttpContextAccessor>();
                    return DateTimeConverter.GetTimeZoneByAspNetHeader(httpContextAccessor, timezoneHeaderName);
                }));
            });
        }
    }
}
