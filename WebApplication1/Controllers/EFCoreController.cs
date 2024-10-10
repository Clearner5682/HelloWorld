using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models.EFCoreModels;

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
    }
}
