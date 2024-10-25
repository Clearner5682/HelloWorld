using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.DependencyInject.Services
{
    public class SmtpEmailSender:IEmailSender
    {
        public void SendEmail()
        {
            Console.WriteLine("Send email by Smtp...");
        }
    }
}
