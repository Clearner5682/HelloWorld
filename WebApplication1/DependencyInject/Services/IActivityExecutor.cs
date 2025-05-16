using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace WebApplication1.DependencyInject.Services
{
    public interface IActivityExecutor
    {
        Task ExecuteActivity(ActivityExecutionContext context);
    }
}
