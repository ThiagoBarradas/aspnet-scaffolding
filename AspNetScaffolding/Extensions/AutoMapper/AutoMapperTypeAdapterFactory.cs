using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyModel;

namespace AspNetScaffolding.Extensions.AutoMapper
{
    public class AutoMapperTypeAdapterFactory : ITypeAdapterFactory
    {
        private readonly IMapper mapper;

        public AutoMapperTypeAdapterFactory()
        {
            var profiles = GetAssemblies()
                .SelectMany(p => p.GetTypes())
                .Where(p => p.GetTypeInfo().BaseType == typeof(Profile));

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.AllowNullDestinationValues = true;
                foreach (var profile in profiles)
                {
                    cfg.AddProfile(Activator.CreateInstance(profile) as Profile);
                }
            });


            configuration.AssertConfigurationIsValid();
            
            mapper = configuration.CreateMapper();
        }

        public ITypeAdapter Create()
        {
            return new AutoMapperTypeAdapter(mapper);
        }

        private Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries.Where(p =>
                p.Type.Equals("Project", StringComparison.CurrentCultureIgnoreCase));
            foreach (var library in dependencies)
            {
                var name = new AssemblyName(library.Name);
                var assembly = Assembly.Load(name);
                assemblies.Add(assembly);
            }

            return assemblies.ToArray();
        }
    }
}