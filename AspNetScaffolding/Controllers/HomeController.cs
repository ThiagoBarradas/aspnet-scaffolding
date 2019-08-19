using AspNetScaffolding.Extensions.AccountId;
using AspNetScaffolding.Extensions.RequestKey;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AspNetScaffolding.Controllers
{
    public class HomeController : BaseController
    {
        protected IHostingEnvironment HostingEnvironment { get; set; }

        protected RequestKey RequestKey { get; set; }

        public HomeController(IHostingEnvironment hostingEnvironment, RequestKey requestKey, AccountId accountId)
        {
            this.RequestKey = requestKey;
            this.HostingEnvironment = hostingEnvironment;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(HomeDetails), 200)]
        public IActionResult Home()
        {
            return Ok(new HomeDetails
            {
                Service = Api.ApiBasicConfiguration?.ApiName,
                BuildVersion = Api.ApiSettings?.BuildVersion,
                Environment = this.HostingEnvironment.EnvironmentName,
                RequestKey = this.RequestKey.Value
            });
        }

        public class HomeDetails
        {
            public string Service { get; set; }

            public string BuildVersion { get; set; }

            public string Environment { get; set; }

            public string RequestKey { get; set; }
        }
    }
}
