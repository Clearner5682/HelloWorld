using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.DependencyInject.Services;
using WebApplication1.EFCore;
using WebApplication1.EFCore.Models.TestDb20240721;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DependencyInjectController : ControllerBase
    {
        private readonly IJobEngine jobEngine;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly MyDbContext dbContext;

        public DependencyInjectController(IJobEngine jobEngine,IServiceScopeFactory serviceScopeFactory,MyDbContext dbContext)
        {
            this.jobEngine = jobEngine;
            this.serviceScopeFactory = serviceScopeFactory;
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            await jobEngine.StartJob();

            return Ok();
        }

        [HttpGet]
        [Route("Test22")]
        public async Task<IActionResult> Test22()
        {
            Task.Run(async () =>
            {
                Thread.Sleep(2000);
                IServiceScope scope = this.serviceScopeFactory.CreateScope();

                try
                {
                    var context = scope.ServiceProvider.GetService<MyDbContext>();

                    var list = await context.Set<TorrentInfo>().Where(o => o.CreatedDate > DateTime.Now.AddMonths(-5)).ToListAsync();
                }
                catch (Exception ex)
                {

                }

                scope.Dispose();
            });

            return Ok();
        }
    }
}
