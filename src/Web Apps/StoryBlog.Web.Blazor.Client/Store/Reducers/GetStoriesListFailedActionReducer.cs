using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    public sealed class GetStoriesListFailedActionReducer : Reducer<BlogState, GetStoriesListFailedAction>
    {
        public override BlogState Reduce(BlogState state, GetStoriesListFailedAction action)
        {
            return new BlogState(false, state.Stories, action.Error);
        }
    }
}