using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    public sealed class GetStoriesListActionReducer : Reducer<BlogState, GetStoriesListAction>
    {
        public override BlogState Reduce(BlogState state, GetStoriesListAction action)
        {
            return new BlogState(true, state.Stories, null);
        }
    }
}
