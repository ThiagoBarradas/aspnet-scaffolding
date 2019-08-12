using AspNetScaffolding.Extensions.JsonSerializer;
using AspNetScaffolding.Models;
using Microsoft.Extensions.DependencyInjection;
using PackUtils;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace AspNetScaffolding.Extensions.Docs
{
    public static class DocsServiceExtension
    {
        public static DocsSettings DocsSettings { get; set; }

        public static void SetupSwaggerDocs(
            this IServiceCollection services,
            DocsSettings docsSettings,
            ApiSettings apiSettings)
        {
            DocsSettings = docsSettings;

            if (DocsSettings?.Enabled == true)
            {
                DocsSettings.Version = apiSettings.Version;
                DocsSettings.PathPrefix = apiSettings.PathPrefix;
                GenerateSwaggerUrl();

                services.AddSwaggerGen(options =>
                {
                    string readme = null;
                    if (string.IsNullOrWhiteSpace(DocsSettings.PathToReadme) == false)
                    {
                        readme = File.ReadAllText(DocsSettings.PathToReadme);
                    }

                    switch (apiSettings.JsonSerializer)
                    {
                        case JsonSerializerEnum.Camelcase:
                            options.SchemaFilter<CamelEnumSchemaFilter>();
                            break;
                        case JsonSerializerEnum.Snakecase:
                            options.SchemaFilter<SnakeEnumSchemaFilter>();
                            break;
                        case JsonSerializerEnum.Lowercase:
                            options.SchemaFilter<LowerEnumSchemaFilter>();
                            break;
                    }

                    options.OperationFilter<QuerystringCaseOperationFilter>();
                    options.DescribeAllEnumsAsStrings();
                    options.SwaggerDoc(apiSettings.Version, new Info
                    {
                        Title = DocsSettings.Title,
                        Version = apiSettings.Version,
                        Description = readme,
                        Contact = new Contact
                        {
                            Name = DocsSettings.AuthorName,
                            Email = DocsSettings.AuthorEmail
                        }
                    });
                });
            }
        }

        private static void GenerateSwaggerUrl()
        {
            string swaggerJsonPath = "/swagger/{documentName}/swagger.json";
            string finalPath = string.Format("/swagger/{0}/swagger.json", DocsServiceExtension.DocsSettings.Version);
            string docsPath = "/docs";

            if (DocsServiceExtension.DocsSettings.PathPrefix?.Contains("{version}", StringComparison.OrdinalIgnoreCase) == true)
            {
                swaggerJsonPath = DocsServiceExtension.DocsSettings.PathPrefix.Replace("{version}", "{documentName}", StringComparison.OrdinalIgnoreCase).Trim('/');
                swaggerJsonPath = string.Format("/{0}/swagger.json", swaggerJsonPath);
                finalPath = swaggerJsonPath.Replace("{documentName}", DocsServiceExtension.DocsSettings.Version);
                docsPath = finalPath.Replace("swagger.json", "docs");
            }
            else if (string.IsNullOrWhiteSpace(DocsServiceExtension.DocsSettings?.PathPrefix) == false)
            {
                var prefix = string.Format("/{0}/", DocsServiceExtension.DocsSettings.PathPrefix.Trim('/'));
                swaggerJsonPath = prefix + swaggerJsonPath.TrimStart('/');
                finalPath = prefix + finalPath.TrimStart('/');
                docsPath = prefix + docsPath.TrimStart('/');
            }

            DocsServiceExtension.DocsSettings.SwaggerJsonTemplateUrl = swaggerJsonPath;
            DocsServiceExtension.DocsSettings.SwaggerJsonUrl = finalPath;
            DocsServiceExtension.DocsSettings.RedocUrl = docsPath;
        }
    }
}
