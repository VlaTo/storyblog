using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LandingFeature : Feature<LandingState>
    {
        public override string GetName() => nameof(LandingState);

        protected override LandingState GetInitialState() => LandingState.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class LandingState
    {
        public static readonly LandingState Empty;

        public bool IsBusy
        {
            get;
        }

        public LandingModel Model
        {
            get;
        }

        public string Error
        {
            get;
        }

        private LandingState(bool isBusy, LandingModel model, string error)
        {
            IsBusy = isBusy;
            Model = model;
            Error = error;
        }

        static LandingState()
        {
            Empty = new LandingState(false, null, null);
        }

        public static LandingState Loading(LandingModel model)
        {
            return new LandingState(true, model, null);
        }

        public static LandingState Success(LandingModel model)
        {
            return new LandingState(false, model, null);
        }

        public static LandingState Failed(LandingModel model, string error)
        {
            return new LandingState(false, model, error);
        }
    }
}