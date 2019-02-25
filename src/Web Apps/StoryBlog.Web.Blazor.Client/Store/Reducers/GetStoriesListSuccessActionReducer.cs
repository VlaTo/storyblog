using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    public sealed class GetStoriesListSuccessActionReducer : Reducer<BlogState, GetStoriesListSuccessAction>
    {
        public override BlogState Reduce(BlogState state, GetStoriesListSuccessAction action)
            => new BlogState(false, action.Stories, null);
    }
}