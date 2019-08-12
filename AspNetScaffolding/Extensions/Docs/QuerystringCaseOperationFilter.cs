using AspNetScaffolding.Extensions.JsonSerializer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace AspNetScaffolding.Extensions.Docs
{
    public class QuerystringCaseOperationFilter : IOperationFilter
    {
        public QuerystringCaseOperationFilter()
        {
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                foreach (var param in operation.Parameters.Where(p => p.In == "query"))
                {
                    param.Name = param.Name.GetValueConsideringCurrentCase();
                }
            }
        }
    }
}
