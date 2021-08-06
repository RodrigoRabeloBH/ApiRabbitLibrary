using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerController : ControllerBase
    {
        private readonly IConsumerServices _services;
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(IConsumerServices services, ILogger<ConsumerController> logger)
        {
            _services = services;
            _logger = logger;
        }

        [HttpGet("{queueName}")]
        public IActionResult GetMessagesFromQueue(string queueName)
        {
            try
            {
                var messages = _services.GetMessages(queueName);

                if (messages.Any())
                {
                    return Ok(messages);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult ConfirmDeliveryMessage(IEnumerable<Message> messages)
        {
            try
            {
                _services.ConfirmDeliveryMessage(messages);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);

                return StatusCode(500);
            }
        }
    }
}
