using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AspNetScaffolding.Controllers
{
    public class HomeController : BaseController
    {
        protected IHostingEnvironment HostingEnvironment { get; set; }

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
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
                Environment = this.HostingEnvironment.EnvironmentName
            });
        }

        public class HomeDetails
        {
            public string Service { get; set; }

            public string BuildVersion { get; set; }

            public string Environment { get; set; }
        }
    }
}
