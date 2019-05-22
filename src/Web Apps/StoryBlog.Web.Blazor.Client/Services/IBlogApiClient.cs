using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System;
using System.Collections.Generic;
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
        /// <returns></returns>
        Task<LandingModel> GetLandingAsync(LandingIncludes flags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        Task<EntityListResult<FeedStory>> GetStoriesAsync(StoryIncludes include);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<EntityListResult<FeedStory>> GetStoriesFromAsync(Uri requestUri);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        Task<StoryModel> GetStoryAsync(string slug, StoryIncludes flags);

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
        Task<IEnumerable<RubricModel>> GetRubricsAsync();
    }
}