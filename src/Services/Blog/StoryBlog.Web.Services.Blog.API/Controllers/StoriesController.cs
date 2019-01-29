using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Common;
using StoryBlog.Web.Services.Blog.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;

namespace StoryBlog.Web.Services.Blog.API.Controllers
{
    /// <summary>
    /// Feed of stories controller.
    /// </summary>
    //[Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class StoriesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger logger;

        public StoriesController(
            IMediator mediator,
            IMapper mapper,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            blogSettings = storyBlogSettings.Get(String.Empty);
            logger = loggerFactory.CreateLogger<StoriesController>();
        }

        // GET api/v1/stories
        [HttpPost]
        [ProducesResponseType(typeof(StoryModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateStoryModel model)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(new CreateStoryCommand(User, model.Title, String.Empty));

            if (false == result.IsSuccess())
            {
                ;
            }

            return Ok();
        }

        // GET api/v1/stories
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery(Name = "include")] string include)
        {
            var includes = include.Split(',');
            var stories = await mediator.Send(new GetStoriesListQuery(User, blogSettings.PageSize, includes));

            return Ok(new ListResult<StoryModel>
            {
                Data = stories.Select(story => mapper.Map<StoryModel>(story)),
                Meta = new ResultMetaInformation
                {
                    Navigation = new Navigation
                    {
                        Previous = Url.Action("Get", "Stories", new {prev = "111"}),
                        Next = Url.Action("Get", "Stories", new {next = "222"})
                    }
                }
            });
        }
    }
}
