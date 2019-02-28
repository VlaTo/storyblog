using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStorySuccessAction : IAction
    {
        /// <summary>
        /// 
        /// </summary>
        public StoryModel Story
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public GetStorySuccessAction(StoryModel story)
        {
            Story = story;
        }
    }
}