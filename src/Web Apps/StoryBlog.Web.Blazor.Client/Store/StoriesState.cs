using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoriesFeature : Feature<StoriesState>
    {
        public override string GetName() => nameof(StoriesState);

        protected override StoriesState GetInitialState() =>
            new StoriesState(ModelStatus.None, new StoryModel[0]);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class StoriesState
    {
        public ModelStatus Status
        {
            get;
        }
        
        public StoryModel[] Stories
        {
            get;
        }

        public StoriesState(ModelStatus status, StoryModel[] stories)
        {
            Status = status;
            Stories = stories;
        }
    }
}