using StoryBlog.Web.Services.Blog.Interop;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System.Threading.Tasks;

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
        Task<ListResult<StoryModel, ResourcesMeta>> GetStoriesAsync(StoryIncludes include);

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
    }
}