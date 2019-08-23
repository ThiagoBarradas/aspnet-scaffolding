using AspNetScaffolding.Models;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace AspNetScaffolding.DemoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ApiBasicConfiguration
            {
                ApiName = "My AspNet Scaffolding",
                ApiPort = 8700,
                EnvironmentVariablesPrefix = "Prefix_",
                ConfigureMapper = AdditionalConfigureMapper,
                ConfigureHealthcheck = AdditionalConfigureHealthcheck,
                ConfigureServices = AdditionalConfigureServices,
                Configure = AdditionalConfigure,
                AutoRegisterAssemblies = new Assembly[] 
                    { Assembly.GetExecutingAssembly() }
            };

            Api.Run(config);
        }

        public static void AdditionalConfigureHealthcheck(IHealthChecksBuilder builder, IServiceCollection services)
        {
            // add health check configuration
            builder.AddUrlGroup(new Uri("https://www.google.com"), "google");
            //builder.AddMongoDb("mongodb://localhost:27017");
        }

        public static void AdditionalConfigureServices(IServiceCollection services)
        {
            // add services
            //services.AddSingleton<ISomething, Something>();
        }

        public static void AdditionalConfigure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // customize your app
            //app.UseAuthentication();
        }

        public static void AdditionalConfigureMapper(IMapperConfigurationExpression mapper)
        {
            // customize your mappers
            //mapper.CreateMap<SomeModel, OtherModel>();
        }
    }
}
