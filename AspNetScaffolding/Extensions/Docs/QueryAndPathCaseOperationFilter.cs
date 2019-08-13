using AspNetScaffolding.Extensions.JsonSerializer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace AspNetScaffolding.Extensions.Docs
{
    public class QueryAndPathCaseOperationFilter : IOperationFilter
    {
        public QueryAndPathCaseOperationFilter()
        {
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                foreach (var param in operation.Parameters.Where(p => p.In == "query" || p.In == "path"))
                {
                    param.Name = param.Name.GetValueConsideringCurrentCase();
                }

                var grouped = operation.Parameters
                    .Where(p => p.In == "query" || p.In == "path")
                    .GroupBy(r => r.Name);

                var queryAndPath = grouped.Select(r => r.OrderBy(p => p.In).First()).ToList();

                operation.Parameters.ToList()
                    .RemoveAll(p => p.In == "query" || p.In == "path");

                operation.Parameters.ToList().AddRange(queryAndPath);
            }

            if (context.ApiDescription.ParameterDescriptions != null)
            {
                foreach (var param in context.ApiDescription.ParameterDescriptions)
                {
                    param.Name = param.Name.GetValueConsideringCurrentCase();
                }
            }
        }
    }
}
