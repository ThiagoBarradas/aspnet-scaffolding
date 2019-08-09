using AspNetScaffolding.Extensions.JsonSerializer;
using PackUtils;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace AspNetScaffolding.Extensions.Docs
{
    public class QuerystringCaseOperationFilter : IOperationFilter
    {
        public JsonSerializerEnum JsonSerializerMode { get; set; }

        public QuerystringCaseOperationFilter(JsonSerializerEnum jsonSerializerMode)
        {
            this.JsonSerializerMode = jsonSerializerMode;
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters != null)
            {
                foreach (var param in operation.Parameters.Where(p => p.In == "query"))
                {
                    param.Name = this.GetNewValue(param.Name);
                }
            }
        }

        private string GetNewValue(string value)
        {
            switch (this.JsonSerializerMode)
            {
                case JsonSerializerEnum.Camelcase:
                    value = value.ToCamelCase();
                    break;
                case JsonSerializerEnum.Snakecase:
                    value = value.ToSnakeCase();
                    break;
                case JsonSerializerEnum.Lowercase:
                    value = value.ToLowerCase();
                    break;
            }

            return value;
        }
    }
}
