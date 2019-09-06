﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Extensions;
using StoryBlog.Web.Services.Blog.API.Infrastructure;
using StoryBlog.Web.Services.Blog.API.Models;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Interop.Core;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using StoryBlog.Web.Services.Shared.Communication;
using StoryBlog.Web.Services.Shared.Communication.Commands;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Navigation = StoryBlog.Web.Services.Blog.Interop.Models.Navigation;

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
        private readonly IStringLocalizer<StoriesController> localizer;
        private readonly IPluralService pluralService;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger<StoriesController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="commandBus"></param>
        /// <param name="localizer"></param>
        /// <param name="settingsAccessor"></param>
        /// <param name="logger"></param>
        public StoriesController(
            IMediator mediator,
            IMapper mapper,
            ICommandBus commandBus,
            IStringLocalizer<StoriesController> localizer,
            IPluralService pluralService,
            IOptionsSnapshot<StoryBlogSettings> settingsAccessor,
            ILogger<StoriesController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.commandBus = commandBus;
            this.localizer = localizer;
            this.pluralService = pluralService;
            this.logger = logger;

            blogSettings = settingsAccessor.Value;
        }

        // POST api/v1/stories
        [Authorize]
        //[AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(Models.StoryModel), (int) HttpStatusCode.OK)]
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

            if (result.IsFailed)
            {
                return BadRequest();
            }

            await commandBus.SendAsync(new StoryCreatedIntegrationCommand
            {
                Id = Guid.NewGuid(),
                StoryId = result.Entity.Id,
                Sent = result.Entity.Created
            });

            logger.StoryCreated(result.Entity.Id);

            return Created(
                Url.Action("Get", "Story", new {slug = result.Entity.Slug}),
                mapper.Map<Models.StoryModel>(result.Entity)
            );
        }

        /// <summary>
        /// Gets available stories chunk based on <paramref name="page" /> specified.
        /// </summary>
        /// <param name="page">The navigation token.</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{page?}")]
        [ProducesResponseType(typeof(GetStoriesActionModel), (int) HttpStatusCode.OK)]
        //public async Task<IActionResult> Get(string page, [FromCommaSeparatedQuery(Name = "include", EnumType = typeof(StoryFlags))] IEnumerable<string> includes)
        public async Task<IActionResult> Get(string page, [FromQuery(Name = "include")] StoryFlags includes)
        {
            var query = new GetStoriesQuery(User)
            {
                IncludeAuthors = StoryFlags.Authors == (includes & StoryFlags.Authors),
                IncludeComments = StoryFlags.Comments == (includes & StoryFlags.Comments),
                Cursor = (null != page && NavigationCursorEncoder.TryParse(page, out var cursor))
                    ? cursor
                    : NavigationCursor.Forward(0, blogSettings.PageSize)
            };

            var result = await mediator.Send(query, HttpContext.RequestAborted);

            if (result.IsFailed)
            {
                return BadRequest();
            }

            var include = Flags.Format(typeof(StoryFlags), StoryFlags.Authors, "F");
            string forward = null;
            string backward = null;

            if (null != result.Backward)
            {
                backward = Url.Action(nameof(Get), "Stories", new
                {
                    page = NavigationCursorEncoder.ToEncodedString(result.Backward), include
                });
            }

            if (null != result.Forward)
            {
                forward = Url.Action(nameof(Get), "Stories", new
                {
                    page = NavigationCursorEncoder.ToEncodedString(result.Forward), include
                });
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

            return Ok(new GetStoriesActionModel
            {
                Data = result
                    .Select(story =>
                    {
                        var storyModel1 = mapper.Map<Interop.Models.StoryModel>(story);

                        storyModel1.Author = FindAuthorIndex(story.Author);
                        storyModel1.Comments = story.Comments
                            .Select(comment =>
                            {
                                var commentModel1 = mapper.Map<Interop.Models.CommentModel>(comment);

                                commentModel1.Author = FindAuthorIndex(comment.Author);

                                return commentModel1;
                            })
                            .ToArray();

                        return storyModel1;
                    })
                    .ToArray(),
                Meta = new MetaInfo<Navigation>
                {
                    Resources = new StoryResources
                    {
                        Authors = authors
                            .Select(author => mapper.Map<Interop.Models.AuthorModel>(author))
                            .ToArray()

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