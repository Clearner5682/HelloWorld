using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

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
            var userName = "danny_hong";
            var query = this.myDbContext.Set<UserEmail>().FromSqlRaw($"exec dbo.SP_GetEmail @UserName={userName}");

            var result = await query.Where(o=>o.UserName=="jack").ToListAsync();

            return Ok();
        }
    }
}
