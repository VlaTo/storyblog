using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Extensions;
using StoryBlog.Web.Services.Blog.Application.Comments.Commands;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Communication;
using StoryBlog.Web.Services.Shared.Communication.Commands;
using System;
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
    public sealed class CommentsController : ControllerBase
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
        public CommentsController(
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

        // POST api/v1/comments/<story-slug>
        //[Authorize]
        [AllowAnonymous]
        [HttpPost("{slug:required}")]
        [ProducesResponseType(typeof(CommentModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create(string slug, [FromBody] CreateCommentModel model)
        {
            if (false == ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(
                new CreateCommentCommand(User, slug, model.Content, model.IsPublic),
                HttpContext.RequestAborted
            );

            if (result.IsFailed)
            {
                return BadRequest();
            }

            await commandBus.SendAsync(new CommentCreatedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                StorySlug = slug,
                CommentId = result.Entity.Id,
                Sent = result.Entity.Created
            });

            logger.CommentCreated(slug, result.Entity.Id);

            return Created(
                Url.Action("Get", "Comment", new {id = result.Entity.Id}),
                mapper.Map<CommentModel>(result.Entity)
            );
        }
    }
}