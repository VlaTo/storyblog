using System;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using StoryBlog.Web.Blazor.Client.Store.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Converters;

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
                Comments = CreateCommentsThree(action)
            };

        private static IReadOnlyCollection<CommentBase> CreateCommentsThree(GetStorySuccessAction action)
        {
            CommentBase Project(Models.Data.Comment comment, Comment parent)
            {
                var model = new Comment(action.Slug, parent)
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
                action.Comments
                    .Where(y => y.ParentId.HasValue && parent.Id == y.ParentId.Value)
                    .Select(comment => Project(comment, parent))
                    .ToList()
                    .AsReadOnly();

            var comments = action.Comments
                .Select(comment => Project(comment, null))
                .ToList();

            if (false == action.IsCommentsClosed)
            {
                var reference = Guid.NewGuid();
                comments.Add(new ComposeComment(action.Slug, null, reference));
            }

            return comments.AsReadOnly();
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
    public sealed class ComposeCommentReplyActionReducer : Reducer<StoryState, ComposeCommentReplyAction>
    {
        /// <inheritdoc cref="Reducer{TState,TAction}.Reduce" />
        public override StoryState Reduce(StoryState state, ComposeCommentReplyAction action) =>
            new StoryState(state.Status)
            {
                Slug = state.Slug,
                Title = state.Title,
                Author = state.Author,
                Content = state.Content,
                Published = state.Published,
                IsCommentsClosed = state.IsCommentsClosed,
                CommentsCount = state.CommentsCount,
                Comments = AddComposingComment(state.Comments, action)
            };

        private static IReadOnlyCollection<CommentBase> AddComposingComment(
            IEnumerable<CommentBase> comments,
            ComposeCommentReplyAction action)
        {
            IList<CommentBase> Project(IEnumerable<CommentBase> collection)
            {
                var result = new List<CommentBase>();

                foreach (var child in collection)
                {
                    if (child is Comment comment)
                    {
                        var clone = new Comment(comment.StorySlug, comment.Parent)
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            Author = comment.Author,
                            Published = comment.Published
                        };

                        result.Add(clone);

                        var children = Project(comment.Comments);

                        if (action.ParentId.HasValue && comment.Id == action.ParentId.Value)
                        {
                            children.Add(new ComposeComment(action.StorySlug, comment, action.Reference));
                        }

                        clone.Comments = new ReadOnlyCollection<CommentBase>(children);

                        continue;
                    }

                    if (child is ComposeComment compose)
                    {
                        if (compose.Parent?.Id != action.ParentId)
                        {
                            continue;
                        }
                    }

                    result.Add(child);
                }

                return result;
            }

            var list = Project(comments);

            if (false == action.ParentId.HasValue)
            {
                list.Add(new ComposeComment(action.StorySlug, null, action.Reference));
            }

            return new ReadOnlyCollection<CommentBase>(list);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SaveNewCommentActionReducer : Reducer<StoryState, SaveNewCommentAction>
    {
        public override StoryState Reduce(StoryState state, SaveNewCommentAction action) =>
            new StoryState(ModelStatus.Loading)
            {
                Slug = state.Slug,
                Title = state.Title,
                Author = state.Author,
                Content = state.Content,
                Published = state.Published,
                IsCommentsClosed = state.IsCommentsClosed,
                CommentsCount = state.CommentsCount,
                Comments = ReplaceComposingComment(state.Comments, action)
            };

        private static IReadOnlyCollection<CommentBase> ReplaceComposingComment(
            IEnumerable<CommentBase> comments,
            SaveNewCommentAction action)
        {
            if (false == action.ParentId.HasValue)
            {
                var result = new List<CommentBase>();

                foreach (var child in comments)
                {
                    if (child is ComposeComment compose && action.Reference == compose.Reference)
                    {
                        result.Add(new SavingComment(action.StorySlug, compose.Parent, action.Reference));
                        continue;
                    }

                    result.Add(child);
                }

                return result.AsReadOnly();
            }

            IReadOnlyCollection<CommentBase> Project(IEnumerable<CommentBase> collection)
            {
                var result = new List<CommentBase>();

                foreach (var child in collection)
                {
                    if (child is Comment comment)
                    {
                        result.Add(new Comment(action.StorySlug, comment.Parent)
                        {
                            Id = comment.Id,
                            Author = comment.Author,
                            Content = comment.Content,
                            Published = comment.Published,
                            Comments = Project(comment.Comments)
                        });
                        continue;
                    }

                    if (child is ComposeComment compose && action.Reference == compose.Reference)
                    {
                        result.Add(new SavingComment(action.StorySlug, compose.Parent, action.Reference));
                        continue;
                    }

                    result.Add(child);
                }

                return result.AsReadOnly();
            }

            return Project(comments);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PendingCommentCreatedActionReducer : Reducer<StoryState, PendingCommentCreatedAction>
    {
        public override StoryState Reduce(StoryState state, PendingCommentCreatedAction action) =>
            new StoryState(ModelStatus.Success)
            {
                Slug = state.Slug,
                Title = state.Title,
                Author = state.Author,
                Content = state.Content,
                Published = state.Published,
                IsCommentsClosed = state.IsCommentsClosed,
                CommentsCount = state.CommentsCount + 1,
                Comments = ReplacePendingComments(state.Comments, action)
            };

        private static IReadOnlyCollection<CommentBase> ReplacePendingComments(
            IEnumerable<CommentBase> comments,
            PendingCommentCreatedAction action)
        {
            if (false == action.ParentId.HasValue)
            {
                var result = new List<CommentBase>();

                foreach (var child in comments)
                {
                    if (child is SavingComment saving && action.Reference == saving.Reference)
                    {
                        result.Add(new PendingComment(action.StorySlug, saving.Parent, action.Reference));
                        continue;
                    }

                    result.Add(child);
                }

                return result.AsReadOnly();
            }

            IReadOnlyCollection<CommentBase> Project(IEnumerable<CommentBase> collection)
            {
                var result = new List<CommentBase>();

                foreach (var child in collection)
                {
                    if (child is Comment comment)
                    {
                        result.Add(new Comment(action.StorySlug, comment.Parent)
                        {
                            Id = action.Id,
                            Content = action.Content,
                            Author = action.Author,
                            Published = action.Published,
                            Comments = Project(comment.Comments)
                        });
                        continue;
                    }

                    if(child is SavingComment saving && action.Reference == saving.Reference)
                    {
                        result.Add(new PendingComment(action.StorySlug, saving.Parent, action.Reference));
                        continue;
                    }

                    result.Add(child);
                }

                return result.AsReadOnly();
            }

            return Project(comments);
        }
    }
}