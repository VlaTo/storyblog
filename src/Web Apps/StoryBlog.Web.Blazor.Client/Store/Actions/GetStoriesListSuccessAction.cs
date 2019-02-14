using System.Collections.Generic;
using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Common.Models;

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