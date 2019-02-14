using StoryBlog.Web.Services.Blog.Common;
using StoryBlog.Web.Services.Blog.Common.Models;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Services
{
    public interface IBlogApiClient
    {
        Task<ListResult<StoryModel>> GetStoriesAsync();
    }
}
