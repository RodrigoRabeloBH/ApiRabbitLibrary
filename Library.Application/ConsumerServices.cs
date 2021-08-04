using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Library.Application
{
    public class ConsumerServices : IConsumerServices
    {
        private readonly IRabbitServices _services;
        private readonly ILogger<ConsumerServices> _logger;

        public ConsumerServices(IRabbitServices services, ILogger<ConsumerServices> logger)
        {
            _services = services;
            _logger = logger;
        }

        public void ConfirmDeliveryMessage(IEnumerable<Message> messages)
        {
            try
            {
                _services.ConfirmDelivery(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }

        }

        public IEnumerable<Message> GetMessages(string queueName)
        {
            IEnumerable<Message> messages = null;

            try
            {
                messages = _services.ReceiverMessage(queueName);

            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }
            return messages;
        }
    }
}
