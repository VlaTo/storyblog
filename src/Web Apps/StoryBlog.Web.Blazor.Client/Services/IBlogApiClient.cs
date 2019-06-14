using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IBlogApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<LandingModel> GetLandingAsync(LandingIncludes flags, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="include"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityListResult<FeedStory>> GetStoriesAsync(StoryIncludes include, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<EntityListResult<FeedStory>> GetStoriesFromAsync(Uri requestUri, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Story> GetStoryAsync(string slug, StoryIncludes flags, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        Task<bool> CreateStoryAsync(StoryModel story);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RubricModel>> GetRubricsAsync(CancellationToken cancellationToken);
    }
}