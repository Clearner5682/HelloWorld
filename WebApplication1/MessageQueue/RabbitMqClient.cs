using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication1.MessageQueue
{
    public class RabbitMqClient
    {
        private readonly IModel _channel;
        private readonly ILogger _logger;
        private string _exchangeType;
        private string _clientName;
        private string _exchangeName;
        private string _deadLetterClientName;
        private string _deadLetterExchangeName;


        public RabbitMqClient(IOptions<RabbitMqEventBusOptions> options, ILogger<RabbitMqClient> logger)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = options.Value.HostName,
                };
                var connection = factory.CreateConnection();
                _channel = connection.CreateModel();
            }
            catch (Exception ex)
            {
                logger.LogError(-1, ex, "RabbitMqClient init fail");
            }
            _logger = logger;
            _exchangeType = options.Value.ExchangeType;
            _clientName = options.Value.Normal.ClientName;
            _exchangeName = options.Value.Normal.ExchangeName;
            _deadLetterClientName = options.Value.DeadLetter.ClientName;
            _deadLetterExchangeName = options.Value.DeadLetter.ExchangeName;
        }

        public virtual void PushMessage(string routingKey,string deadLetterRoutingKey, object message)
        {
            _logger.LogInformation($"PushMessage,routingKey:{routingKey}");
            _channel.ExchangeDeclare(exchange: _exchangeName, type: _exchangeType, durable: true);
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            arguments.Add("x-dead-letter-exchange", _deadLetterExchangeName);
            arguments.Add("x-dead-letter-routing-key", deadLetterRoutingKey);
            _channel.QueueDeclare(queue: _clientName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: arguments);
            string msgJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(msgJson);
            _channel.BasicPublish(exchange: _exchangeName,
                                    routingKey: routingKey,
                                    basicProperties: null,
                                    body: body);
        }
    }
}
