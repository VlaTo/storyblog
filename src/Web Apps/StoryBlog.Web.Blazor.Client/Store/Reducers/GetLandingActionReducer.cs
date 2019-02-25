using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    public sealed class GetLandingActionReducer : Reducer<LandingState, GetLandingAction>
    {
        public override LandingState Reduce(LandingState state, GetLandingAction action)
            => new LandingState(true, state.Data, null);
    }
}