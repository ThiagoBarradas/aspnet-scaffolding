using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AspNetScaffolding.Models
{
    public class ApiBasicConfiguration
    {
        public string ApiName { get; set; }

        public int ApiPort { get; set; }

        public string EnvironmentVariablesPrefix { get; set; }

        public IEnumerable<Assembly> AutoRegisterAssemblies { get; set; } = new List<Assembly>();

        public Func<IServiceCollection> ConfigureServices { get; set; }

        public Func<IApplicationBuilder, IHostingEnvironment> Configure { get; set; }
    }
}
