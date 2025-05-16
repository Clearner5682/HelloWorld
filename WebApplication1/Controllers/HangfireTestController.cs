using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApplication1.Hangfire;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireTestController : ControllerBase
    {
        private readonly IBackgroundJobClient backgroundJobClient;
        private readonly ILogger<HangfireTestController> logger;

        public HangfireTestController(IBackgroundJobClient backgroundJobClient)
        {
            this.backgroundJobClient = backgroundJobClient;
        }

        [HttpGet]
        [Route("AddFireAndForget")]
        public async Task<IActionResult> AddFireAndForget()
        {
            //this.backgroundJobClient.Enqueue(() =>Console.WriteLine("Hello world from Hangfire!"));
            this.backgroundJobClient.Enqueue(() => TestMethod());

            await Task.CompletedTask;

            return Ok();
        }

        public static void TestMethod()
        {
            throw new Exception("An error occurs");
        }

        [HttpGet]
        [Route("AddDelayed")]
        public async Task<IActionResult> AddDelayed()
        {
            this.backgroundJobClient.Schedule(() => Console.WriteLine("Delayed!"), TimeSpan.FromSeconds(10));

            await Task.CompletedTask;

            return Ok();
        }

        [HttpGet]
        [Route("AddRecurring")]
        public async Task<IActionResult> AddRecurring()
        {
            RecurringJob.AddOrUpdate("recurring1", () => Console.WriteLine("Executed."), Cron.Minutely());

            await Task.CompletedTask;

            return Ok();
        }

        [HttpGet]
        [Route("AddEmailSender")]
        public async Task<IActionResult> AddEmailSender()
        {
            this.backgroundJobClient.Enqueue<EmailSender>(sender => sender.SendEmail());

            await Task.CompletedTask;

            return Ok();
        }
    }
}
