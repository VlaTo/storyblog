using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListAction : IAction
    {
        public StoryIncludes Includes
        {
            get;
        }

        public GetStoriesListAction(StoryIncludes includes)
        {
            Includes = includes;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListFailedAction : IAction
    {
        public string Error
        {
            get;
        }

        public GetStoriesListFailedAction(string error)
        {
            Error = error;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListSuccessAction : IAction
    {
        public IEnumerable<StoryModel> Stories
        {
            get;
        }

        public GetStoriesListSuccessAction(IEnumerable<StoryModel> stories)
        {
            Stories = stories;
        }
    }
}
