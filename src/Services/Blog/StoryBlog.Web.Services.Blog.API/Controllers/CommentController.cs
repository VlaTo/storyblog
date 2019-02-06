using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.Application.Extensions;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.Common.Models;
using StoryBlog.Web.Services.Blog.Infrastructure;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Services.Blog.API.Controllers
{
    /// <summary>
    /// Feed of stories controller.
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class CommentController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ICommandBus commandBus;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger<CommentsController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="commandBus"></param>
        /// <param name="storyBlogSettings"></param>
        /// <param name="logger"></param>
        public CommentController(
            IMediator mediator,
            IMapper mapper,
            ICommandBus commandBus,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILogger<CommentsController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.commandBus = commandBus;
            this.logger = logger;

            blogSettings = storyBlogSettings.Value;
        }

        // GET api/v1/comment/<id>
        [AllowAnonymous]
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(CommentModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(long id, [FromCommaSeparatedQuery(Name = "include")] IEnumerable<string> includes)
        {
            var flags = FlagParser.Parse<IncludeFlags>(includes);
            var result = await mediator.Send(new GetStoryQuery(User, slug)
            {
                IncludeAuthors = flags.IncludeAuthors,
                IncludeComments = flags.IncludeComments
            });

            if (false == result.IsSuccess())
            {
                return NotFound();
            }

            return Ok(mapper.Map<StoryModel>(result.Data));
        }
    }
}