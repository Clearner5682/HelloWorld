using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace WebApplication1.Controllers
{
    [Route("api/File")]
    [ApiController]
    public class FileController : ControllerBase
    {
        #region 上传文件

        [HttpPost, Route("Upload")]
        public IActionResult Upload(string name)
        {
            IFormFileCollection formCollection = this.Request.Form.Files;
            var file = formCollection[0];
            string fileName = file.FileName;
            long fileSize = file.Length;

            return Ok(fileName + "=>" + fileSize);
        }

        #endregion

        #region 文件流的操作

        [HttpGet, Route("CopyFile")]
        public IActionResult CopyFile()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyToByFile("copy.txt", "save.txt");
            fileHelper.CopyToByFile("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyFileAsync")]
        public async Task<IActionResult> CopyFileAsync()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyToByFile("copy.txt", "save.txt");
            await fileHelper.CopyToByFileAsync("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyBySelf")]
        public IActionResult CopyBySelf()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyBySelf("copy.txt", "save.txt");
            fileHelper.CopyBySelf("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyByMemory")]
        public IActionResult CopyByMemory()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyByMemory("copy.txt", "save.txt");
            fileHelper.CopyByMemory("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyByStream")]
        public IActionResult CopyByStream()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyByStream("copy.txt", "save.txt");
            fileHelper.CopyByStream("copy.jpg", "save.jpg");

            return Ok();
        }

        [HttpGet, Route("CopyByByte")]
        public IActionResult CopyByByte()
        {
            var fileHelper = new FileHelper();
            //fileHelper.CopyByByte("copy.txt", "save.txt");
            fileHelper.CopyByByte("copy.jpg", "save.jpg");

            return Ok();
        }

        #endregion
    }
}
