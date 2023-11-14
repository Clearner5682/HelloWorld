using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace WebApplication1.MessageQueue
{
    public class MyNormalListener : RabbitMqListener
    {
        
        private readonly ILogger<MyNormalListener> logger;

        public MyNormalListener(IOptions<RabbitMqEventBusOptions> options, ILogger<MyNormalListener> logger, IServiceProvider serviceProvider) : base(options)
        {
            this.logger = logger;
            this.RoutingKey = "NormalKey";
            this.DeadLetterRoutingKey = "DeadLetterKey";
        }

        public override async Task<bool> Process(string message)
        {
            Thread.Sleep(2000);
            this.logger.LogWarning($"{message}");
            await Task.CompletedTask;

            int id = JsonConvert.DeserializeObject<int>(message);
            if (id % 2 == 0)
            {
                return false;
            }

            return true;
        }
    }
}
