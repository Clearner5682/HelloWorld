using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace WebApplication1.DependencyInject.Services
{
    public class JobEngine : IJobEngine
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IActivityExecutor activityExecutor;

        public JobEngine(IServiceScopeFactory serviceScopeFactory,IActivityExecutor activityExecutor)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.activityExecutor = activityExecutor;
        }

        public async Task StartJob()
        {
            await this.activityExecutor.ExecuteActivity(new ActivityExecutionContext { Count=3 });
        }

        public static async Task ActivitySucceedHandler(ActivityExecutionContext context,IServiceScope serviceScope)
        {
            if (context.Count > 0)
            {
                context.Count = context.Count - 1;

                IActivityExecutor executor = serviceScope.ServiceProvider.GetService<IActivityExecutor>();
                await executor.ExecuteActivity(context);
            }
        }
    }
}
