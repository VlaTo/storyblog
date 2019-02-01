using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Common.Models;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Controllers
{
    /// <summary>
    /// Feed of stories controller.
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class StoryController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="storyBlogSettings"></param>
        /// <param name="loggerFactory"></param>
        public StoryController(
            IMediator mediator,
            IMapper mapper,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            blogSettings = storyBlogSettings.Value;
            logger = loggerFactory.CreateLogger<StoriesController>();
        }

        // GET api/v1/stories/<slug>
        [AllowAnonymous]
        [HttpGet("{slug:required}")]
        [ProducesResponseType(typeof(StoryModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string slug, [FromCommaSeparatedQuery(Name = "include")] IEnumerable<string> includes)
        {
            var flags = FlagParser.Parse<IncludeFlags>(includes);
            var story = await mediator.Send(new GetStoryQuery(User, slug)
            {
                IncludeAuthors = flags.IncludeAuthors,
                IncludeComments = flags.IncludeComments
            });

            if (null == story)
            {
                return NotFound();
            }

            return Ok(story);
        }
    }
}