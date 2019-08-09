using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class StoryFeature : Feature<StoryState>
    {
        public override string GetName() => nameof(StoryState);

        protected override StoryState GetInitialState() => new StoryState(ModelStatus.None);
    }

    /// <summary>
    /// 
    /// </summary>
    public class StoryState : IHasModelStatus
    {
        public ModelStatus Status
        {
            get;
        }

        public Story Story
        {
            get;
        }

        public StoryState(ModelStatus status, Story value = null)
        {
            Status = status;
            Story = value;
        }
    }
}