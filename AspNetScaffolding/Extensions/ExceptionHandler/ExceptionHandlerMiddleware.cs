using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using WebApi.Models.Exceptions;
using WebApi.Models.Helpers;

namespace AspNetScaffolding.Extensions.ExceptionHandler
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate Next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.Next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is ApiException)
            {
                return ApiException(context, (ApiException) exception);
            }
            else
            {
                return GenericError(context, exception);
            }
        }

        private static Task GenericError(HttpContext context, Exception exception)
        {
            context.Items.Add("Exception", exception);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            return Task.CompletedTask;
        }

        private static Task ApiException(HttpContext context, ApiException exception)
        {
            var apiResponse = exception.ToApiResponse();

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) apiResponse.StatusCode;

            if (apiResponse.Content != null)
            {
                context.Response.WriteAsync(JsonConvert.SerializeObject(apiResponse.Content)).Wait();
                context.Response.Body.Position = 0;
            }

            return Task.CompletedTask;
        }
    }

    public static class ExceptionHandlerMiddlewareExtension
    {
        public static void UseScaffoldingExceptionHandler(this IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionHandlerMiddleware>();
            }
        }
    }
}
