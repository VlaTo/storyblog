using System;
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
    public sealed class GetStoriesListActionReducer : Reducer<StoriesState, GetStoriesAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesAction action)
            => new StoriesState(ModelStatus.Loading, state.Stories);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListFailedActionReducer : Reducer<StoriesState, GetStoriesFailedAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesFailedAction action)
            => new StoriesState(ModelStatus.Failed(action.Error), state.Stories);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListSuccessActionReducer : Reducer<StoriesState, GetStoriesSuccessAction>
    {
        public override StoriesState Reduce(StoriesState state, GetStoriesSuccessAction action)
        {
            var stories = new List<StoryModel>();

            foreach (var story in action.Stories)
            {
                var model = new StoryModel
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Content = story.Content,
                    AllCommentsCount = story.Comments.Length,
                    Published = DateTime.Now //story.Modified.GetValueOrDefault(story.Created)
                };

                Comments.CreateCommentsTree(model.Comments, story.Comments);

                stories.Add(model);
            }

            return new StoriesState(ModelStatus.Success, stories.ToArray());
        }
    }
}
