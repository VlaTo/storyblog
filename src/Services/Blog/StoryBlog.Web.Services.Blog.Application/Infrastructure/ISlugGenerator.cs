namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISlugGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        string CreateFrom(string title);
    }
}