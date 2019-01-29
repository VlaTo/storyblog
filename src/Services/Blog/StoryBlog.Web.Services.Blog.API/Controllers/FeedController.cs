using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Domain;
using StoryBlog.Web.Services.Blog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using StoryBlogSettings = StoryBlog.Web.Services.Blog.API.Infrastructure.StoryBlogSettings;

namespace StoryBlog.Web.Services.Blog.API.Controllers
{
    /// <summary>
    /// Feed of stories controller.
    /// </summary>
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger<FeedController> logger;

        public FeedController(
            IMediator mediator,
            IMapper mapper,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILoggerFactory loggerFactory)
        {
            this.mediator = mediator;
            this.mapper = mapper;

            blogSettings = storyBlogSettings.Get(String.Empty);
            logger = loggerFactory.CreateLogger<FeedController>();
        }

        // GET api/v1/feed
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FeedStory>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery] int page)
        {
            var stories = await mediator.Send(new GetFeedStoryListQuery(User));
            return Ok(stories.Select(story => mapper.Map<FeedStory>(story)));
        }
    }
}