using System.Collections.Generic;
using System.Linq;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class StoriesFeature : Feature<StoriesState>
    {
        public override string GetName() => nameof(StoriesState);

        protected override StoriesState GetInitialState() =>
            new StoriesState(ModelStatus.None, Enumerable.Empty<FeedStory>(), null);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class StoriesState : IHasModelStatus
    {
        public ModelStatus Status
        {
            get;
        }

        public IEnumerable<FeedStory> Stories
        {
            get;
        }

        public Navigation Navigation
        {
            get;
        }

        public StoriesState(ModelStatus status, IEnumerable<FeedStory> stories, Navigation navigation)
        {
            Status = status;
            Stories = stories;
            Navigation = navigation;
        }
    }
}