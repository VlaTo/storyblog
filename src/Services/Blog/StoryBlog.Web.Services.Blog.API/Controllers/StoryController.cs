using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Extensions;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.Application.Extensions;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using StoryBlog.Web.Services.Shared.Communication;
using StoryBlog.Web.Services.Shared.Communication.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Shared.Infrastructure.Extensions;
using StoryBlog.Web.Services.Blog.Interop;

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
        [ProducesResponseType(typeof(Result<StoryModel, ResourcesMetaInfo<AuthorsResource>>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string slug, [FromCommaSeparatedQuery(Name = "include")] IEnumerable<string> includes)
        {
            var flags = EnumFlags.Parse<StoryIncludes>(includes);
            var query = new GetStoryQuery(User, slug)
            {
                IncludeAuthors = StoryIncludes.Authors == (flags & StoryIncludes.Authors),
                IncludeComments = StoryIncludes.Comments == (flags & StoryIncludes.Comments)
            };

            var result = await mediator.Send(query, HttpContext.RequestAborted);

            if (false == result.IsSuccess())
            {
                return NotFound();
            }

            return Ok(new Result<StoryModel, ResourcesMetaInfo<AuthorsResource>>
            {
                Data = mapper.Map<StoryModel>(result.Data),
                Meta = new ResourcesMetaInfo<AuthorsResource>
                {
                    Resources = new AuthorsResource
                    {
                        Authors=
                    }
                }
            });
        }

        // PUT api/v1/story/<slug>
        [AllowAnonymous]
        [HttpPut("{slug:required}")]
        [ProducesResponseType(typeof(StoryModel), (int) HttpStatusCode.OK)]
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

            if (false == result.IsSuccess())
            {
                return BadRequest(result.Exceptions);
            }

            await commandBus.SendAsync(new StoryUpdatedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                StoryId = result.Data.Id,
                Sent = dateTimeProvider.Now
            });

            logger.StoryUpdated(result.Data.Id);

            if (slug != result.Data.Slug)
            {
                var url = Url.Action("Get", "Story", new {slug = result.Data.Slug});
            }

            return Ok(mapper.Map<StoryModel>(result.Data));
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

            if (false == result.IsSuccess())
            {
                return BadRequest(result.Exceptions);
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