using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryFeature : Feature<StoryState>
    {
        public override string GetName() => nameof(StoryState);

        protected override StoryState GetInitialState() => new StoryState(false, null, null);
    }

    /// <summary>
    /// 
    /// </summary>
    public class StoryState
    {
        public bool IsBusy
        {
            get;
        }

        public StoryModel Story
        {
            get;
        }

        public string Error
        {
            get;
        }

        public StoryState(bool busy, StoryModel story, string error)
        {
            IsBusy = busy;
            Story = story;
            Error = error;
        }
    }
}