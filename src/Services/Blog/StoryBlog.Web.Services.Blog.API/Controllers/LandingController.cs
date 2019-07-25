using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.API.Models;
using StoryBlog.Web.Services.Blog.API.Models.Results.Resources;
using StoryBlog.Web.Services.Blog.Application.Landing.Queries;
using StoryBlog.Web.Services.Blog.Interop.Core;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class LandingController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly StoryBlogSettings blogSettings;
        private readonly ILogger<StoriesController> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="mapper"></param>
        /// <param name="storyBlogSettings"></param>
        /// <param name="logger"></param>
        public LandingController(
            IMediator mediator,
            IMapper mapper,
            IOptionsSnapshot<StoryBlogSettings> storyBlogSettings,
            ILogger<StoriesController> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;

            blogSettings = storyBlogSettings.Value;
        }

        // GET api/v1/landing?include=hero,featured,stories
        //[Authorize(Policy = "Default")]
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(LandingModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery(Name = "include")]IEnumerable<string> includes)
        {
            //var flags = Enums.Parse<LandingIncludes>(includes);
            var query = new GetLandingQuery
            {
                //IncludeHeroStory = LandingIncludes.HeroStory == (flags & LandingIncludes.HeroStory),
                IncludeHeroStory = false,
                //FeaturedStoriesCount = LandingIncludes.FeaturedStories == (flags & LandingIncludes.FeaturedStories)
                //    ? blogSettings.FeaturedStoriesCount
                //    : 0
                FeaturedStoriesCount = 0
                //StoriesFeedCount = LandingIncludes.StoriesFeed == (flags & LandingIncludes.StoriesFeed)
                //    ? blogSettings.FeedStoriesCount
                //    : 0
            };

            var result = await mediator.Send(query, HttpContext.RequestAborted);

            if (result.IsFailed)
            {
                return BadRequest();
            }

            //return Ok(mapper.Map<LandingModel>(result.Data));

            return Ok(new LandingModel
            {
                Title = "",
                Description = "",
                Data = Enumerable.Empty<FeedStoryModel>(),
                Hero = new HeroStoryModel(),
                Featured = Enumerable.Empty<FeedStoryModel>(),
                Meta = new ResourcesMetaInfo<AuthorsResource>
                {
                    Resources = new AuthorsResource
                    {
                        Authors = Enumerable.Empty<Models.AuthorModel>()
                    }
                }
            });
        }
    }
}