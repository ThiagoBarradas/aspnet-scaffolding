using Microsoft.AspNetCore.Builder;
using System;

namespace AspNetScaffolding.Extensions.Docs
{
    public static class DocsMiddlewareExtension
    {
        public static void UseScaffoldingSwagger(this IApplicationBuilder app)
        {
            if (DocsServiceExtension.DocsSettings?.Enabled == true)
            {
                GenerateSwaggerUrl();

                var title = DocsServiceExtension.DocsSettings?.Title ?? "API Reference";

                app.UseStaticFiles();

                app.UseSwagger(c =>
                {
                    c.RouteTemplate = DocsServiceExtension.DocsSettings.SwaggerJsonTemplateUrl.TrimStart('/');
                });

                app.UseReDoc(c =>
                {
                    c.RoutePrefix = DocsServiceExtension.DocsSettings.RedocUrl.TrimStart('/');
                    c.SpecUrl = DocsServiceExtension.DocsSettings.SwaggerJsonUrl;
                    c.DocumentTitle = title;
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
