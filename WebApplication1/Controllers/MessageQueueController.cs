using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication1.MessageQueue;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageQueueController : ControllerBase
    {
        private readonly RabbitMqClient rabbitMqClient;

        public MessageQueueController(RabbitMqClient rabbitMqClient)
        {
            this.rabbitMqClient = rabbitMqClient;
        }

        [Route("AddNormal")]
        [HttpGet]
        public async Task<IActionResult> AddNormal(string id)
        {
            await Task.CompletedTask;
            this.rabbitMqClient.PushMessage("NormalKey", "DeadLetterKey", id);

            return Ok();
        }
    }
}
