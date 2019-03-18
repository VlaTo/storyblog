using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using StoryBlog.Web.Blazor.Client.Store.Helpers;
using StoryBlog.Web.Blazor.Client.Store.Models;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListActionReducer : Reducer<StoriesState, GetStoriesListAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesListAction action)
            => new StoriesState(ModelStatus.Loading, state.Stories);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListFailedActionReducer : Reducer<StoriesState, GetStoriesListFailedAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesListFailedAction action)
            => new StoriesState(ModelStatus.Failed(action.Error), state.Stories);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListSuccessActionReducer : Reducer<StoriesState, GetStoriesListSuccessAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesListSuccessAction action)
        {
            var stories = new List<StoryModel>();

            foreach (var story in action.Stories)
            {
                var model = new StoryModel
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Content = story.Content,
                    AllCommentsCount = story.Comments.Length
                };

                Comments.CreateCommentsTree(model.Comments, story.Comments);

                stories.Add(model);
            }

            return new StoriesState(ModelStatus.Success, stories.ToArray());
        }
    }
}
