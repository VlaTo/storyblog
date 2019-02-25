using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    public sealed class GetLandingSuccessActionReducer : Reducer<LandingState, GetLandingSuccessAction>
    {
        public override LandingState Reduce(LandingState state, GetLandingSuccessAction action)
            => new LandingState(false, action.Data, null);
    }
}