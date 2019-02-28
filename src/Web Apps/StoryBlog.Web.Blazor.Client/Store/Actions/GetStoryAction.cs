using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Includes;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryAction : IAction
    {
        /// <summary>
        /// 
        /// </summary>
        public string Slug
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public StoryIncludes Flags
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        public GetStoryAction(string slug, StoryIncludes flags)
        {
            Slug = slug;
            Flags = flags;
        }
    }
}