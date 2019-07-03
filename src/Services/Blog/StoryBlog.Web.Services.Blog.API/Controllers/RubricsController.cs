using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Rubrics.Queries;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Blog.API.Controllers
{
    /// <summary>
    /// Feed of stories controller.
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RubricsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<RubricsController> logger;

        public RubricsController(IMediator mediator, IMapper mapper, ILogger<RubricsController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ListResult<RubricModel, ResultMetaInfo>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var query = new GetRubricsQuery
            {

            };

            var result = await mediator.Send(query);

            if (result.IsFailed)
            {
                return BadRequest();
            }

            return Ok(new ListResult<RubricModel, ResultMetaInfo>
            {
                Data = result.Entity.Select(rubric => mapper.Map<RubricModel>(rubric)),
                Meta = null
            });
        }
    }
}
