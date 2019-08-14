using System;
using System.Collections.Generic;
using System.Linq;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using StoryBlog.Web.Blazor.Client.Store.Models;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryActionReducer : Reducer<StoryState, GetStoryAction>
    {
        public override StoryState Reduce(StoryState state, GetStoryAction action) =>
            new StoryState(ModelStatus.Loading);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStorySuccessActionReducer : Reducer<StoryState, GetStorySuccessAction>
    {
        public override StoryState Reduce(StoryState state, GetStorySuccessAction action) =>
            new StoryState(ModelStatus.Success, action.Story);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryFailedActionReducer : Reducer<StoryState, GetStoryFailedAction>
    {
        public override StoryState Reduce(StoryState state, GetStoryFailedAction action) =>
            new StoryState(ModelStatus.Failed(action.Error));
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateNewCommentActionReducer : Reducer<StoryState, CreateNewCommentAction>
    {
        public override StoryState Reduce(StoryState state, CreateNewCommentAction action)
        {

            return new StoryState(ModelStatus.Loading, state.Story);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreatedActionReducer : Reducer<StoryState, CommentCreatedAction>
    {
        public override StoryState Reduce(StoryState state, CommentCreatedAction action)
        {
            if (false == String.Equals(action.Slug, state.Story.Slug))
            {
                return state;
            }

            if (action.Comment.Parent.HasValue)
            {
                var parent = GetComment(state.Story.Comments, action.Comment.Parent.Value);

                if (null == parent)
                {
                    throw new Exception();
                }

                parent.Comments.Add(new Comment(parent)
                {
                    Id = action.Comment.Id,
                    Content = action.Comment.Content,
                    Author = action.Comment.Author,
                    Published = action.Comment.Published
                });
            }
            else
            {
                state.Story.Comments.Add(new Comment(null)
                {
                    Id = action.Comment.Id,
                    Content = action.Comment.Content,
                    Author = action.Comment.Author,
                    Published = action.Comment.Published
                });
            }

            state.Story.CommentsCount++;

            return new StoryState(ModelStatus.Success, state.Story);
        }

        private Comment GetComment(ICollection<Comment> comments, long id)
        {
            var comment = comments.FirstOrDefault(child => child.Id == id);

            if (null == comment)
            {
                foreach (var child in comments)
                {
                    comment = GetComment(child.Comments, id);

                    if (null == comment)
                    {
                        continue;
                    }

                    break;
                }
            }

            return comment;
        }
    }
}