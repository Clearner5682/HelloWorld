using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;

namespace WebApplication1.MessageQueue
{
    public abstract class RabbitMqListener : IHostedService
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly IConnection normalConnection;
        private readonly IModel normalChannel;
        private readonly IConnection deadLetterConnection;
        private readonly IModel deadLetterChannel;
        private string _exchangeType;
        private string _clientName;
        private string _exchangeName;
        private string _deadLetterClientName;
        private string _deadLetterExchangeName;

        public RabbitMqListener(IOptions<RabbitMqEventBusOptions> options)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = options.Value.HostName,
                };
                this.connection = factory.CreateConnection();
                this.channel = connection.CreateModel();
                this.normalConnection = factory.CreateConnection();
                this.normalChannel = normalConnection.CreateModel();
                this.deadLetterConnection = factory.CreateConnection();
                this.deadLetterChannel = deadLetterConnection.CreateModel();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMqListener init error,ex:{ex.Message}");
            }
            _exchangeType = options.Value.ExchangeType;
            _clientName = options.Value.Normal.ClientName;
            _exchangeName = options.Value.Normal.ExchangeName;
            _deadLetterClientName= options.Value.DeadLetter.ClientName;
            _deadLetterExchangeName= options.Value.DeadLetter.ExchangeName;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }


        protected string RoutingKey;
        protected string DeadLetterRoutingKey;

        // 处理消息的方法
        public virtual Task<bool> Process(string message)
        {
            throw new NotImplementedException();
        }

        // 注册消费者监听在这里
        public void Register()
        {
            Console.WriteLine($"RabbitMqListener register,routingKey:{RoutingKey},deadLetterRoutingKey:{DeadLetterRoutingKey}");

            #region 创建死信交换机和死信队列

            channel.ExchangeDeclare(exchange: _deadLetterExchangeName, type: _exchangeType, durable: true);
            channel.QueueDeclare(queue: _deadLetterClientName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);
            //死信队列绑定死信交换机
            channel.QueueBind(queue: _deadLetterClientName, exchange: _deadLetterExchangeName, routingKey: DeadLetterRoutingKey);

            #endregion

            #region 创建业务交换机和业务队列

            channel.ExchangeDeclare(exchange: _exchangeName, type: _exchangeType, durable: true);
            Dictionary<string, object> arguments = new Dictionary<string, object>();
            arguments.Add("x-dead-letter-exchange", _deadLetterExchangeName);
            arguments.Add("x-dead-letter-routing-key", DeadLetterRoutingKey);
            channel.QueueDeclare(queue: _clientName,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: arguments);
            channel.QueueBind(queue: _clientName, exchange: _exchangeName, routingKey: RoutingKey);

            #endregion

            //channel.BasicQos(0, 5, false);
            var consumer = new EventingBasicConsumer(normalChannel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                var result = await Process(message);
                if (result)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    //channel.BasicNack(ea.DeliveryTag,false,true);
                    //channel.BasicNack(ea.DeliveryTag, false, false);
                    channel.BasicReject(ea.DeliveryTag, false);// 拒绝消息，直接放入死信队列
                }
            };
            channel.BasicConsume(queue: _clientName, consumer: consumer);
        }

        public void DeRegister()
        {
            this.connection.Close();
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.connection.Close();
            return Task.CompletedTask;
        }
    }
}
