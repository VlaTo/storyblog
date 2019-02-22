using StoryBlog.Web.Services.Blog.Common;
using StoryBlog.Web.Services.Blog.Common.Includes;
using StoryBlog.Web.Services.Blog.Common.Models;
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
        /// <param name="include"></param>
        /// <returns></returns>
        Task<ListResult<StoryModel>> GetStoriesAsync(StoryIncludes include);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        Task CreateStoryAsync(StoryModel story);
    }
}