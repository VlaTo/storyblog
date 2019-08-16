using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using StoryBlog.Web.Blazor.Client.Store.Models;
using System.Collections.Generic;
using System.Linq;

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
            new StoryState(ModelStatus.Success)
            {
                Slug = action.Slug,
                Title = action.Title,
                Author = action.Author,
                Content = action.Content,
                Published = action.Published,
                IsCommentsClosed = action.IsCommentsClosed,
                CommentsCount = action.Comments.Count,
                Comments = CreateCommentsThree(action.Comments)
            };

        private static IReadOnlyCollection<CommentBase> CreateCommentsThree(ICollection<Models.Data.Comment> comments)
        {
            Comment Project(Models.Data.Comment comment, Comment parent)
            {
                var model = new Comment(parent)
                {
                    Id = comment.Id,
                    Author = comment.Author,
                    Content = comment.Content,
                    Published = comment.Published
                };

                model.Comments = ProjectChildComments(model);

                return model;
            }

            IReadOnlyCollection<CommentBase> ProjectChildComments(Comment parent) =>
                comments
                    .Where(y => y.ParentId.HasValue && parent.Id == y.ParentId.Value)
                    .Select(comment => Project(comment, parent))
                    .ToList()
                    .AsReadOnly();

            return comments
                .Select(comment => Project(comment, null))
                .ToList()
                .AsReadOnly();
        }
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
    public sealed class CreatePendingCommentActionReducer : Reducer<StoryState, CreatePendingCommentAction>
    {
        public override StoryState Reduce(StoryState state, CreatePendingCommentAction action) =>
            new StoryState(ModelStatus.Loading)
            {
                Slug = state.Slug,
                Title = state.Title,
                Author = state.Author,
                Content = state.Content,
                Published = state.Published,
                IsCommentsClosed = state.IsCommentsClosed,
                CommentsCount = state.CommentsCount,
                Comments = AddPendingComment(state.Comments, action)
            };

        private static IReadOnlyCollection<CommentBase> AddPendingComment(IEnumerable<CommentBase> comments,
            CreatePendingCommentAction action)
        {
            if (false == action.ParentId.HasValue)
            {
                return new List<CommentBase>(comments)
                {
                    new PendingComment(null)
                }.AsReadOnly();
            }

            var parentId = action.ParentId.Value;

            IReadOnlyCollection<CommentBase> Trace(IEnumerable<CommentBase> collection)
            {
                var result = new List<CommentBase>();

                foreach (var child in collection)
                {
                    if (child is Comment comment)
                    {
                        if (comment.Id == parentId)
                        {
                            result.Add(new PendingComment(comment));
                            continue;
                        }

                        comment.Comments = Trace(comment.Comments);
                    }

                    result.Add(child);
                }

                return result.AsReadOnly();
            }

            return Trace(comments);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreatedActionReducer : Reducer<StoryState, CommentCreatedAction>
    {
        public override StoryState Reduce(StoryState state, CommentCreatedAction action) =>
            new StoryState(ModelStatus.Success)
            {
                Slug = state.Slug,
                Title = state.Title,
                Author = state.Author,
                Content = state.Content,
                Published = state.Published,
                IsCommentsClosed = state.IsCommentsClosed,
                CommentsCount = state.CommentsCount + 1,
                Comments = CreateComments(state.Comments, action)
            };

        private static IReadOnlyCollection<CommentBase> CreateComments(IEnumerable<CommentBase> comments,
            CommentCreatedAction action)
        {
            if (false == action.ParentId.HasValue)
            {
                return new List<CommentBase>(comments)
                {
                    new Comment(null)
                    {
                        Id = action.Id,
                        Content = action.Content,
                        Author = action.Author,
                        Published = action.Published
                    }
                }.AsReadOnly();
            }

            var parentId = action.ParentId.Value;

            IReadOnlyCollection<CommentBase> Trace(IEnumerable<CommentBase> collection)
            {
                var result = new List<CommentBase>();

                foreach (var child in collection)
                {
                    if (child is Comment comment)
                    {
                        if (comment.Id == parentId)
                        {
                            result.Add(new Comment(comment)
                            {
                                Id = action.Id,
                                Content = action.Content,
                                Author = action.Author,
                                Published = action.Published
                            });
                            continue;
                        }

                        comment.Comments = Trace(comment.Comments);
                    }

                    result.Add(child);
                }

                return result.AsReadOnly();
            }

            return Trace(comments);
        }
    }
}