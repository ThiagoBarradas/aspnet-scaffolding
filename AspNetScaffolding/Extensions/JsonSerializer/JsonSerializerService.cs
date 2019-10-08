﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PackUtils;
using PackUtils.Converters;
using System;

namespace AspNetScaffolding.Extensions.JsonSerializer
{
    public static class JsonSerializerService
    {
        public static JsonSerializerSettings JsonSerializerSettings { get; set; }

        public static Newtonsoft.Json.JsonSerializer JsonSerializer { get; set; }

        public static void ConfigureJsonSettings(
            this IMvcBuilder mvc, 
            IServiceCollection services,
            JsonSerializerEnum jsonSerializerMode, 
            string timezoneHeaderName,
            TimeZoneInfo defaultTimeZone)
        {
            CaseUtility.JsonSerializerMode = jsonSerializerMode;

            JsonSerializerSettings = null;
            JsonSerializer = null;

            switch (jsonSerializerMode)
            {
                case JsonSerializerEnum.Camelcase:
                    JsonSerializer = JsonUtility.CamelCaseJsonSerializer;
                    JsonSerializerSettings = JsonUtility.CamelCaseJsonSerializerSettings;
                    break;
                case JsonSerializerEnum.Lowercase:
                    JsonSerializer = JsonUtility.LowerCaseJsonSerializer;
                    JsonSerializerSettings = JsonUtility.LowerCaseJsonSerializerSettings;
                    break;
                case JsonSerializerEnum.Snakecase:
                    JsonSerializer = JsonUtility.SnakeCaseJsonSerializer;
                    JsonSerializerSettings = JsonUtility.SnakeCaseJsonSerializerSettings;
                    break;
                default:
                    break;
            }

            JsonSerializer.Converters.Clear();
            JsonSerializer.Converters.Add(new EnumWithContractJsonConverter());
            JsonSerializerSettings.Converters.Clear();
            JsonSerializerSettings.Converters.Add(new EnumWithContractJsonConverter());

            JsonConvert.DefaultSettings = () => JsonSerializerSettings;

            services.AddScoped((provider) => JsonSerializer);
            services.AddScoped((provider) => JsonSerializerSettings);

            DateTimeConverter.DefaultTimeZone = defaultTimeZone;
            mvc.AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = JsonSerializerSettings.ContractResolver;
                options.SerializerSettings.Converters = JsonSerializerSettings.Converters;
                options.SerializerSettings.NullValueHandling = JsonSerializerSettings.NullValueHandling;
                options.SerializerSettings.Converters.Add(new DateTimeConverter(() => {
                    var httpContextAccessor = services.BuildServiceProvider().GetService<IHttpContextAccessor>();
                    return DateTimeConverter.GetTimeZoneByAspNetHeader(httpContextAccessor, timezoneHeaderName);
                }));
            });
        }
    }
}
