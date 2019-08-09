using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.RequestKey
{
    public class RequestKeyMiddleware
    {
        private readonly RequestDelegate Next;

        private RequestKey RequestKey { get; set; }

        public RequestKeyMiddleware(RequestDelegate next, RequestKey requestKey)
        {
            this.RequestKey = requestKey;
            this.Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(RequestKeyServiceExtension.RequestKeyHeaderName))
            {
                this.RequestKey = new RequestKey(context.Request.Headers[RequestKeyServiceExtension.RequestKeyHeaderName]);
            }
            else
            {
                this.RequestKey = new RequestKey(Guid.NewGuid().ToString());
            }

            context.Items.Add(RequestKeyServiceExtension.RequestKeyHeaderName, this.RequestKey.Value);
            context.Response.Headers.Add(RequestKeyServiceExtension.RequestKeyHeaderName, this.RequestKey.Value);

            await this.Next(context);
        }
    }

    public static class RequestKeyMiddlewareExtension
    {
        public static void UseRequestKey(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestKeyMiddleware>();
        }
    }
}
