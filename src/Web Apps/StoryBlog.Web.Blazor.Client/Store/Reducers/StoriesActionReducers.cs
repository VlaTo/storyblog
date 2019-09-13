using Blazor.Fluxor;
using StoryBlog.Web.Client.Store.Actions;
using StoryBlog.Web.Client.Store.Models.Data;
using System.Linq;

namespace StoryBlog.Web.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesActionReducer : Reducer<StoriesState, GetStoriesAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesAction action)
            => new StoriesState(ModelStatus.Loading, Enumerable.Empty<FeedStory>());
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesBackwardActionReducer : Reducer<StoriesState, GetStoriesBackwardAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesBackwardAction action)
            => new StoriesState(ModelStatus.Loading, state.Stories, state.BackwardUri, state.ForwardUri);
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesForwardActionReducer : Reducer<StoriesState, GetStoriesForwardAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesForwardAction action)
            => new StoriesState(ModelStatus.Loading, state.Stories, state.BackwardUri, state.ForwardUri);
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesFailedActionReducer : Reducer<StoriesState, GetStoriesFailedAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesFailedAction action)
            => new StoriesState(ModelStatus.Failed(action.Error), state.Stories);
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesSuccessActionReducer : Reducer<StoriesState, GetStoriesSuccessAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesSuccessAction action)
            => new StoriesState(ModelStatus.Success, action.Stories, action.BackwardUri, action.ForwardUri);
    }
}