using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SlugTextGenerator : ISlugGenerator
    {
        /// <inheritdoc cref="ISlugGenerator.CreateFrom" />
        public string CreateFrom(string title)
        {
            throw new System.NotImplementedException();
        }
    }
}