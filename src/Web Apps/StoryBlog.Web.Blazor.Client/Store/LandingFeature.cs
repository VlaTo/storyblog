using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store
{
    public sealed class LandingFeature : Feature<LandingState>
    {
        public override string GetName() => nameof(LandingState);

        protected override LandingState GetInitialState() => new LandingState(false, null, null);
    }
}