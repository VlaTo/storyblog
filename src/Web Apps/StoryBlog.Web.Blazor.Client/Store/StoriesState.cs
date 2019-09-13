using Blazor.Fluxor;
using StoryBlog.Web.Client.Store.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Client.Store
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

        public Uri BackwardUri
        {
            get;
        }

        public Uri ForwardUri
        {
            get;
        }

        public StoriesState(ModelStatus status, IEnumerable<FeedStory> stories, Uri backwardUri = null, Uri forwardUri = null)
        {
            Status = status;
            Stories = stories;
            BackwardUri = backwardUri;
            ForwardUri = forwardUri;
        }
    }
}