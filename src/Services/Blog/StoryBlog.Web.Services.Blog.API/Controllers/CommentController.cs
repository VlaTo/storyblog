using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Extensions;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.API.Models;
using StoryBlog.Web.Services.Blog.Application.Comments.Commands;
using StoryBlog.Web.Services.Blog.Application.Comments.Queries;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Interop.Core;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Communication;
using StoryBlog.Web.Services.Shared.Communication.Commands;
using System;
using System.Collections.Generic;
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
    public sealed class CommentController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ICommandBus commandBus;
        private readonly IDateTimeProvider dateTimeProvider;
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
            IDateTimeProvider dateTimeProvider,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILogger<CommentsController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.commandBus = commandBus;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;

            blogSettings = storyBlogSettings.Value;
        }

        // GET api/v1/comment/<id>
        [AllowAnonymous]
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(Models.CommentModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(long id, [FromCommaSeparatedQuery(Name = "include")] IEnumerable<string> includes)
        {
            var flags = EnumFlags.Parse<CommentIncludes>(includes);
            var query = new GetCommentQuery(User, id)
            {
                IncludeAuthor = CommentIncludes.Authors == (flags & CommentIncludes.Authors)
            };

            var result = await mediator.Send(query, HttpContext.RequestAborted);

            if (result.IsFailed)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Models.StoryModel>(result.Entity));
        }

        // PUT api/v1/comment/<id>
        [AllowAnonymous]
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(Models.CommentModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Edit(long id, [FromBody] EditCommentModel model)
        {
            if (false == ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(
                new EditCommentCommand(User, id, model.Content, model.IsPublic),
                HttpContext.RequestAborted
            );

            if (result.IsFailed)
            {
                return BadRequest();
            }

            await commandBus.SendAsync(new CommentUpdatedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                //StoryId = result.Data.Id,
                Sent = dateTimeProvider.Now
            });

            logger.StoryUpdated(result.Entity.Id);

            return Ok(mapper.Map<Models.StoryModel>(result.Entity));
        }

        // DELETE api/v1/comment/<id>
        [AllowAnonymous]
        [HttpDelete("{id:long}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await mediator.Send(
                new DeleteCommentCommand(User, id),
                HttpContext.RequestAborted
            );

            if (result.IsFailed)
            {
                return BadRequest();
            }

            await commandBus.SendAsync(new CommentDeletedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                //StoryId = result.Data.Id,
                Sent = dateTimeProvider.Now
            });

            logger.CommentDeleted(id);

            return Ok();
        }
    }
}