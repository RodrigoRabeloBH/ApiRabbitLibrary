using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProducerController : ControllerBase
    {
        private readonly IProducerServices _service;
        private readonly ILogger<ProducerController> _logger;

        public ProducerController(IProducerServices service, ILogger<ProducerController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost("SendCustomer")]
        public async Task<IActionResult> SendCustomer(Customer customer)
        {
            try
            {
                string data = JsonSerializer.Serialize(customer);

                await _service.SendCustomerToQueue(data);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return BadRequest();
            }
        }
    }
}
