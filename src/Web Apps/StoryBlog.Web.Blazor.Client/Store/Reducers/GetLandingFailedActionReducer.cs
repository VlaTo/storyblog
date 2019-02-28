using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    public sealed class GetLandingFailedActionReducer : Reducer<LandingState, GetLandingFailedAction>
    {
        public override LandingState Reduce(LandingState state, GetLandingFailedAction action)
            => LandingState.Failed(state.Model, action.Error);
    }
}