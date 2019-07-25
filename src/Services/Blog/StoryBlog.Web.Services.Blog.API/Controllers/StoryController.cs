using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Extensions;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.API.Models;
using StoryBlog.Web.Services.Blog.API.Models.Results.Resources;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Interop.Core;
using StoryBlog.Web.Services.Shared.Common;
using StoryBlog.Web.Services.Shared.Communication;
using StoryBlog.Web.Services.Shared.Communication.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using StoryBlog.Web.Services.Blog.Interop.Includes;

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
        private readonly ICommandBus commandBus;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger<StoryController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="commandBus"></param>
        /// <param name="storyBlogSettings"></param>
        /// <param name="logger"></param>
        public StoryController(
            IMediator mediator,
            IMapper mapper,
            ICommandBus commandBus,
            IDateTimeProvider dateTimeProvider,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILogger<StoryController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.commandBus = commandBus;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;

            blogSettings = storyBlogSettings.Value;
        }

        // GET api/v1/story/<slug>
        [AllowAnonymous]
        [HttpGet("{slug:required}")]
        [ProducesResponseType(typeof(Result<Models.StoryModel, ResourcesMetaInfo<AuthorsResource>>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string slug, [FromQuery(Name = "include")] IEnumerable<string> includes)
        {
            //var flags = Enums.Parse<StoryFlags>(includes);
            var query = new GetStoryQuery(User, slug)
            {
                //WithAuthors = StoryFlags.Authors == (flags & StoryFlags.Authors),
                WithAuthors = true,
                //WithComments = StoryFlags.Comments == (flags & StoryFlags.Comments)
                WithComments = true
            };

            var result = await mediator.Send(query, HttpContext.RequestAborted);

            if (result.IsFailed)
            {
                return NotFound();
            }

            var authors = new Collection<Author>();

            int FindAuthorIndex(Author author)
            {
                var index = authors.FindIndex(candidate => candidate.Id == author.Id);

                if (0 > index)
                {
                    index = authors.Count;
                    authors.Add(author);
                }

                return index;
            }

            Models.StoryModel MapStory(Story story)
            {
                var storyModel = mapper.Map<Models.StoryModel>(story);

                storyModel.Author = FindAuthorIndex(story.Author);
                storyModel.Comments = story.Comments
                    .Select(comment =>
                    {
                        var commentModel = mapper.Map<Models.CommentModel>(comment);

                        commentModel.Author = FindAuthorIndex(comment.Author);

                        return commentModel;
                    })
                    .ToArray();

                return storyModel;
            }

            return Ok(new Result<Models.StoryModel, ResourcesMetaInfo<AuthorsResource>>
            {
                Data = MapStory(result.Entity),
                Meta = new ResourcesMetaInfo<AuthorsResource>
                {
                    Resources = new AuthorsResource
                    {
                        Authors = authors.Select(author => mapper.Map<Models.AuthorModel>(author))
                    }
                }
            });
        }

        // PUT api/v1/story/<slug>
        [AllowAnonymous]
        [HttpPut("{slug:required}")]
        [ProducesResponseType(typeof(Models.StoryModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Put(string slug, [FromBody] EditStoryModel model)
        {
            if (false == ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(
                new EditStoryCommand(User, slug, model.Title, model.Content, model.IsPublic),
                HttpContext.RequestAborted
            );

            if (result.IsFailed)
            {
                return BadRequest();
            }

            await commandBus.SendAsync(new StoryUpdatedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                StoryId = result.Entity.Id,
                Sent = dateTimeProvider.Now
            });

            logger.StoryUpdated(result.Entity.Id);

            if (slug != result.Entity.Slug)
            {
                var url = Url.Action("Get", "Story", new {slug = result.Entity.Slug});
            }

            return Ok(mapper.Map<Models.StoryModel>(result.Entity));
        }

        // DELETE api/v1/story/<slug>
        [Authorize(Policy = Policies.Admins)]
        //[AllowAnonymous]
        [HttpDelete("{slug:required}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string slug)
        {
            var result = await mediator.Send(
                new DeleteStoryCommand(User, slug),
                HttpContext.RequestAborted
            );

            if (result.IsFailed)
            {
                return BadRequest();
            }

            await commandBus.SendAsync(new StoryDeletedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                StoryId = 1L,
                Sent = dateTimeProvider.Now
            });

            logger.StoryDeleted(1L);

            return Ok();
        }
    }
}