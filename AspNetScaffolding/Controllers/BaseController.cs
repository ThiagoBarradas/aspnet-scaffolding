using AspNetScaffolding.Extensions.Cors;
using AspNetScaffolding.Extensions.JsonSerializer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PackUtils;
using System.IO;
using System.Linq;
using WebApi.Models.Exceptions;
using WebApi.Models.Response;

namespace AspNetScaffolding.Controllers
{
    [EnableCors(CorsServiceExtension.CorsName)]
    public class BaseController : Controller
    {
        public BaseController() {}

        protected IActionResult CreateJsonResponse(ApiResponse response)
        {
            JsonResult result = new JsonResult(response.Content)
            {
                StatusCode = (int)response.StatusCode
            };

            if (response.Headers != null)
            {
                foreach (var header in response.Headers)
                {
                    Response.Headers[header.Key] = header.Value;
                }
            }

            return result;
        }

        protected void ValidateSignatureFromHeaderWithContent(string secretKey, string headerName)
        {
            var result = false;
            var signature = this.Request.Headers[headerName].FirstOrDefault() ?? string.Empty;

            if (this.Request.Body.CanRead)
            {
                using (StreamReader reader = new StreamReader(this.Request.Body))
                {
                    var content = reader.ReadToEnd();
                    result = SignatureUtility.ValidateSignature(signature, secretKey, content);
                }
            }

            if (result == false)
            {
                throw new UnauthorizedException();
            }
        }

        protected void Validate<TRequest>(TRequest request) where TRequest : class, new()
        {
            if (ModelState.IsValid == false)
            {
                ErrorsResponse errors = this.CastModelValidationResultToErrorsResponse(ModelState);
                throw new BadRequestException(errors);
            }
        }

        private ErrorsResponse CastModelValidationResultToErrorsResponse(ModelStateDictionary modelState)
        {
            ErrorsResponse errorsResponse = new ErrorsResponse();

            foreach (var errorPerProperty in modelState)
            {
                foreach (var errorDetail in errorPerProperty.Value.Errors)
                {
                    var propertyName = string.Join(".",
                        errorPerProperty.Key.Split(".")
                        .Select(r => r.GetValueConsideringCurrentCase()));

                    errorsResponse.AddError(propertyName, errorDetail.ErrorMessage);
                }
            }

            return errorsResponse;
        }
    }
}
