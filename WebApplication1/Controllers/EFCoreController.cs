using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.EFCore;
using WebApplication1.EFCore.Models.AIO_Framework;
using WebApplication1.EFCore.Models.TestDb20240721;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EFCoreController : ControllerBase
    {
        private readonly MyDbContext myDbContext;

        public EFCoreController(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpGet]
        [Route("QueryFromSp")]
        public async Task<IActionResult> QueryFromSp()
        {
            var businessType = "ARF";
            var position = "Manager";
            var extraConditions = new List<ExtraCondition>
            {
                new ExtraCondition{ ExtraConditionName="Customer",ExtraConditionValue="008" },
                new ExtraCondition{ ExtraConditionName="Supplier",ExtraConditionValue="043" }
            };
            var query = this.myDbContext
                        .Set<ExtraCondition>()
                        .FromSql($"exec Elsa.SP_GetApproverByExtraConditions @BusinessType={businessType},@Position={position},@ExtraConditions={JsonConvert.SerializeObject(extraConditions)}");

            var result = await query.ToListAsync();

            return Ok();
        }

        [HttpGet]
        [Route("Test1")]
        public async Task<IActionResult> Test1()
        {
            using (var trans = this.myDbContext.Database.BeginTransaction(isolationLevel:System.Data.IsolationLevel.ReadUncommitted))
            {
                var id = new Guid("04F1EA7C-C642-4DF4-9BD7-34CC6416F6C2");
                var toUpdate = await this.myDbContext.Set<DownloadedTorrentInfo>().FirstOrDefaultAsync(o => o.Id == id);
                toUpdate.TorrentFileName = toUpdate.TorrentFileName + "0";

                this.myDbContext.Set<DownloadedTorrentInfo>().Update(toUpdate);
                await this.myDbContext.SaveChangesAsync();

                Thread.Sleep(6000);

                var searchList = await this.myDbContext.Set<DownloadedTorrentInfo>().Where(o => o.TorrentNo == "1111").ToListAsync();

                await trans.CommitAsync();

                return Ok();
            }
        }

        [HttpGet]
        [Route("Test2")]
        public async Task<IActionResult> Test2()
        {
            using (var trans = this.myDbContext.Database.BeginTransaction(isolationLevel: System.Data.IsolationLevel.ReadUncommitted))
            {
                var id = new Guid("4EBFC5B1-6F7B-4AA1-B1EF-D10E7258AD7C");
                var toUpdate = await this.myDbContext.Set<DownloadedTorrentInfo>().FirstOrDefaultAsync(o => o.Id == id);
                toUpdate.TorrentFileName = toUpdate.TorrentFileName + "0";

                var searchList = await this.myDbContext.Set<DownloadedTorrentInfo>().Where(o => o.TorrentNo == "2222").ToListAsync();

                this.myDbContext.Set<DownloadedTorrentInfo>().Update(toUpdate);

                await this.myDbContext.SaveChangesAsync();

                await trans.CommitAsync();

                return Ok();
            }
        }
    }
}
