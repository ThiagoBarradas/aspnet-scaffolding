using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AspNetScaffolding.Extensions.AccountId
{
    public class AccountIdMiddleware
    {
        private readonly RequestDelegate Next;

        private AccountId AccountId { get; set; }

        public AccountIdMiddleware(RequestDelegate next, AccountId accountId)
        {
            this.AccountId = accountId;
            this.Next = next;
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
