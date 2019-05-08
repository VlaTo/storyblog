using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using StoryBlog.Web.Blazor.Client.Store.Helpers;
using StoryBlog.Web.Blazor.Client.Store.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesActionReducer : Reducer<StoriesState, GetStoriesAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesAction action)
            => new StoriesState(ModelStatus.Loading, Enumerable.Empty<FeedStory>(), null);
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesFailedActionReducer : Reducer<StoriesState, GetStoriesFailedAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesFailedAction action)
            => new StoriesState(ModelStatus.Failed(action.Error), state.Stories, state.Navigation);
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesSuccessActionReducer : Reducer<StoriesState, GetStoriesSuccessAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesSuccessAction action)
            => new StoriesState(ModelStatus.Success, action.Stories, action.Navigation);
    }
}
