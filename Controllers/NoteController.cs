using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NoteApplication;
using System.Collections.Generic;

namespace NoteService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetNodeInfo()
        {
            //加载配置文件
            try
            {
                ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

                //添加配置文件路径
                configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var configuration = configurationBuilder.Build();
                string baseDir = configuration.GetValue<string>("NoteHomeDir");
                List<NoteMenu> meuns = new NoteInfoApplication().GetMarkdownNoteFiles(baseDir);

                return new JsonResult(APIResponse<List<NoteMenu>>.Success(meuns));
            }
            catch (Exception ex)
            {
                return new JsonResult(APIResponse<object>.Failure(ex.Message));
            }
        }
    }
}
