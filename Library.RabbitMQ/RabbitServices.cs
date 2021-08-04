using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.RabbitMQ
{
    public class RabbitServices : IRabbitServices
    {
        private static IConnection Connection { get; set; }
        private static IModel Channel { get; set; }
        private BasicGetResult Result { get; set; }

        private readonly ILogger<RabbitServices> _logger;
        private RabbitMqConfiguration _rabbitMqOptions { get; }

        public RabbitServices(ILogger<RabbitServices> logger, IOptions<RabbitMqConfiguration> rabbitMqOptions)
        {
            _logger = logger;
            _rabbitMqOptions = rabbitMqOptions.Value;
        }

        private void CreateConnectionRabbitMq()
        {
            if (Connection == null)
            {
                AbrirConexaoRabbit();
            }
            else
            {
                if (!Connection.IsOpen)
                {
                    AbrirConexaoRabbit();
                }
            }
        }
        private void AbrirConexaoRabbit()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitMqOptions.Hostname,
                UserName = _rabbitMqOptions.UserName,
                Password = _rabbitMqOptions.Password
            };

            Connection = factory.CreateConnection();
        }
        public async Task<bool> SendMessageAsync(string messagem, Contexto contexto)
        {
            try
            {
                CreateConnectionRabbitMq();

                if (Channel == null)
                {
                    Channel = Connection.CreateModel();
                }
                else
                {
                    if (Channel.IsClosed)
                    {
                        Channel = Connection.CreateModel();
                    }
                }
                if (Channel.IsOpen)
                {
                    Channel.QueueDeclare(queue: contexto.ToString(), durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(messagem);

                    Channel.BasicPublish(exchange: string.Empty, routingKey: contexto.ToString(), basicProperties: null, body: body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return await Task.FromResult(true);
        }
        public IEnumerable<Message> ReceiverMessage(string queueName)
        {
            List<Message> messages = new List<Message>();

            CreateConnectionRabbitMq();

            if (Channel == null) Channel = Connection.CreateModel();

            else
            {
                if (Channel.IsClosed) Channel = Connection.CreateModel();
            }

            try
            {
                if (Channel.IsOpen)
                {
                    var msgCount = Channel.MessageCount(queueName);

                    for (int i = 0; i <= msgCount; i++)
                    {
                        Result = null;

                        Result = Channel.BasicGet(queueName, false);

                        if (Result != null)
                        {
                            IBasicProperties Props = Result.BasicProperties;
                            ReadOnlyMemory<byte> body = Result.Body;
                            var messageReceiver = Encoding.UTF8.GetString(body.ToArray());

                            messages.Add(
                                new Message
                                {
                                    DeliveryTag = Result.DeliveryTag,
                                    Content = messageReceiver,
                                });
                        }
                    }
                }
            }
            catch (RabbitMQClientException e)
            {
                _logger.LogError(e.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Channel.BasicNack(Result.DeliveryTag, false, true);
            }
            return messages;
        }
        public void ConfirmDelivery(IEnumerable<Message> messages)
        {
            foreach (var message in messages)
            {
                if (message.Executed)
                {
                    Channel.BasicAck(message.DeliveryTag, false);
                }
                else
                {
                    Channel.BasicNack(message.DeliveryTag, false, true);
                }
            }
        }
    }
}
