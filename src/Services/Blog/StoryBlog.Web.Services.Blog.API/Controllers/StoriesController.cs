﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Extensions;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Interop;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using StoryBlog.Web.Services.Shared.Communication;
using StoryBlog.Web.Services.Shared.Communication.Commands;
using StoryBlog.Web.Services.Shared.Infrastructure.Extensions;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using StoryBlog.Web.Services.Blog.Application.Extensions;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.API.Controllers
{
    /// <summary>
    /// Feed of stories controller.
    /// </summary>
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class StoriesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ICommandBus commandBus;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger<StoriesController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="commandBus"></param>
        /// <param name="storyBlogSettings"></param>
        /// <param name="logger"></param>
        public StoriesController(
            IMediator mediator,
            IMapper mapper,
            ICommandBus commandBus,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILogger<StoriesController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.commandBus = commandBus;
            this.logger = logger;

            blogSettings = storyBlogSettings.Value;
        }

        // POST api/v1/stories
        //[Authorize]
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(StoryModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateStoryModel model)
        {
            if (false == ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await mediator.Send(
                new CreateStoryCommand(User, model.Title, model.Content, model.IsPublic),
                HttpContext.RequestAborted
            );

            if (false == result.IsSuccess())
            {
                foreach (var exception in result.Exceptions)
                {
                    logger.LogError(exception, "[StoriesController.Create]");
                }

                return BadRequest(result.Exceptions);
            }

            await commandBus.SendAsync(new StoryCreatedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                StoryId = result.Data.Id,
                Sent = result.Data.Created
            });

            logger.StoryCreated(result.Data.Id);

            return Created(
                Url.Action("Get", "Story", new {slug = result.Data.Slug}),
                mapper.Map<StoryModel>(result.Data)
            );
        }

        // GET api/v1/stories
        [AllowAnonymous]
        [HttpGet("{page?}")]
        [ProducesResponseType(typeof(ListResult<StoryModel, ResourcesMetaInfo>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string page, [FromCommaSeparatedQuery(Name = "include")] IEnumerable<string> includes)
        {
            var flags = EnumFlags.Parse<StoryIncludes>(includes);
            var query = new GetStoriesListQuery(User)
            {
                IncludeAuthors = StoryIncludes.Authors == (flags & StoryIncludes.Authors),
                IncludeComments = StoryIncludes.Comments == (flags & StoryIncludes.Comments),
                Cursor = (null != page && NavigationCursorEncoder.TryParse(page, out var cursor))
                    ? cursor
                    : NavigationCursor.Forward(0, blogSettings.PageSize)
            };

            var result = await mediator.Send(query, HttpContext.RequestAborted);

            if (false == result.IsSuccess())
            {
                foreach (var exception in result.Exceptions)
                {
                    logger.LogError(exception, "[StoriesController.Get]");
                }

                return BadRequest(result.Exceptions);
            }

            var include = EnumFlags.ToQueryString(flags);
            string backward = null;
            string forward = null;

            if (null != result.Backward)
            {
                backward = Url.Action("Get", "Stories", new
                {
                    page = NavigationCursorEncoder.ToEncodedString(result.Backward),
                    include = include.ToString()
                });
            }

            if (null != result.Forward)
            {
                forward = Url.Action("Get", "Stories", new
                {
                    page = NavigationCursorEncoder.ToEncodedString(result.Forward),
                    include = include.ToString()
                });
            }

            return Ok(new ListResult<StoryModel, ResourcesMetaInfo>
            {
                Data = result.Select(story =>
                {
                    var model = mapper.Map<StoryModel>(story);

                    model.Author = result.Authors.FindIndex(story.Author);

                    return model;
                }),
                Meta = new ResourcesMetaInfo
                {
                    Resources = new AuthorsResource
                    {
                        Authors = result.Authors.Select(author => mapper.Map<AuthorModel>(author))
                    },
                    Navigation = new Navigation
                    {
                        Previous = backward,
                        Next = forward
                    }
                }
            });
        }
    }
}