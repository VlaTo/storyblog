using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Services.Identity.API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    //[ApiController]
    public class PolicyController : Controller
    {
        [HttpGet("show")]
        public async Task<IActionResult> Show()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}