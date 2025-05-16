using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.EFCore;
using WebApplication1.EFCore.Models.TestDb20240721;

namespace WebApplication1.DependencyInject.Services
{
    public class ActivityExecutor : IActivityExecutor
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<ActivityExecutor> logger;
        private readonly MyDbContext dbContext;

        public ActivityExecutor(IServiceScopeFactory serviceScopeFactory, ILogger<ActivityExecutor> logger, MyDbContext dbContext)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public static event Func<ActivityExecutionContext, IServiceScope, Task> OnActivitySucceed;

        public async Task ExecuteActivity(ActivityExecutionContext context)
        {
            await Task.CompletedTask;

            var list = await this.dbContext.Set<TorrentInfo>().Where(o=>o.CreatedDate.HasValue&&o.CreatedDate>new DateTime(2025,3,1)).ToListAsync();

            this.logger.LogWarning($"Executed at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}, Count: {context.Count}");

            using (IServiceScope scope = serviceScopeFactory.CreateScope())
            {

                if (OnActivitySucceed != null)
                {
                    await OnActivitySucceed(context, scope);
                }
            }
        }
    }
}
