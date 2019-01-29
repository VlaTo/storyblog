using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Services.Identity.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (null != User)
            {
                ;
            }

            await Task.CompletedTask;

            return Ok();
        }
    }
}