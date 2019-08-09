using AspNetScaffolding.Models;
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
                AutoRegisterAssemblies = new Assembly[] 
                    { Assembly.GetExecutingAssembly() }
            };

            Api.Run(config);
        }
    }
}
