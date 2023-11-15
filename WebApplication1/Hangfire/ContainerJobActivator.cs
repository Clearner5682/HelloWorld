using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebApplication1.Hangfire
{
    public class ContainerJobActivator:JobActivator
    {
        private readonly ServiceProvider serviceProvider;

        public ContainerJobActivator(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            return this.serviceProvider.GetService(jobType);
        }
    }
}
