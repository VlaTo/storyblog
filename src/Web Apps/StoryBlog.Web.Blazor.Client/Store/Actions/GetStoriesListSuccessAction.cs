using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
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