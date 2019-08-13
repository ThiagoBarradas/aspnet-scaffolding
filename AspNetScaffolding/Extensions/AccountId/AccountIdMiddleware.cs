using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RestSharp.Serilog.Auto;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.AccountId
{
    public class AccountIdMiddleware
    {
        private readonly RequestDelegate Next;

        private IRestClientFactory RestClientFactory { get; set; }

        private AccountId AccountId { get; set; }

        public AccountIdMiddleware(RequestDelegate next, AccountId accountId, IRestClientFactory restClientFactory)
        {
            this.AccountId = accountId;
            this.Next = next;
            this.RestClientFactory = restClientFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            await this.Next(context);

            context.Items.Add(AccountIdServiceExtension.AccountIdHeaderName, this.AccountId.Value);
            context.Response.Headers.Add(AccountIdServiceExtension.AccountIdHeaderName, this.AccountId.Value);
        }
    }

    public static class AccountIdMiddlewareExtension
    {
        public static void UseAccountId(this IApplicationBuilder app)
        {
            app.UseMiddleware<AccountIdMiddleware>();
        }
    }
}
