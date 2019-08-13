using AspNetScaffolding.DemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Response;

namespace AspNetScaffolding.Controllers
{
    public class CustomerController : BaseController
    {
        public CustomerController()
        {

        }

        [HttpGet("customers/{customerId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorsResponse), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(CustomerRequest request)
        {
            return Ok(request);
        }

        [HttpGet("customers/{customerId}/string")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorsResponse), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetString([FromRoute] string customerId)
        {
            return Ok(new { customerId });
        }

        [HttpGet("customers/{customerId}/none")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorsResponse), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetNone(CustomerRequest2 request)
        {
            return Ok(request);
        }

        [HttpGet("customers/{customerId}/fromroute")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorsResponse), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetFromRoute([FromRoute] CustomerRequest2 request)
        {
            return Ok(request);
        }
    }
}
