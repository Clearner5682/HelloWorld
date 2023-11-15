using Microsoft.Extensions.Logging;

namespace WebApplication1.Hangfire
{
    public class EmailSender
    {
        private readonly ILogger<EmailSender> logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            this.logger = logger;
        }

        public void SendEmail()
        {
            this.logger.LogWarning("Email is sent.");
        }
    }
}
