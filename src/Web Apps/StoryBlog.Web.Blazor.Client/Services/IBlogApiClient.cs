using System.Collections.Generic;
using StoryBlog.Web.Services.Blog.Interop;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBlogApiClient
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
        Task<ListResult<StoryModel, ResourcesMetaInfo>> GetStoriesAsync(StoryIncludes include);

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