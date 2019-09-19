using Blazor.Fluxor;
using StoryBlog.Web.Client.Store.Actions;
using StoryBlog.Web.Client.Store.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace StoryBlog.Web.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoryActionReducer : Reducer<StoryState, GetStoryAction>
    {
        public override StoryState Reduce(StoryState state, GetStoryAction action)
        {
            //Debug.WriteLine("GetStoryActionReducer.Reduce status: Loading");
            return new StoryState(ModelStatus.Loading);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStorySuccessActionReducer : Reducer<StoryState, GetStorySuccessAction>
    {
        public override StoryState Reduce(StoryState state, GetStorySuccessAction action)
        {
            //Debug.WriteLine("GetStorySuccessActionReducer.Reduce status: Success");
            return new StoryState(ModelStatus.Success)
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
        }

        private static IReadOnlyCollection<CommentBase> CreateCommentsThree(GetStorySuccessAction action)
        {
            List<CommentBase> Project(Comment parent)
            {
                var session = Guid.NewGuid();
                var collection = new List<CommentBase>();

                foreach (var comment in action.Comments)
                {
                    var shouldSkip = null != parent
                        ? comment.ParentId != parent.Id
                        : comment.ParentId.HasValue;

                    if (shouldSkip)
                    {
                        continue;
                    }

                    var model = new Comment(action.Slug, parent)
                    {
                        Id = comment.Id,
                        Author = comment.Author,
                        Content = comment.Content,
                        Published = comment.Published
                    };

                    model.Comments = Project(model).AsReadOnly();

                    collection.Add(model);
                }

                return collection;
            }

            var list = Project(null);

            if (false == action.IsCommentsClosed)
            {
                list.Add(new ComposeComment(action.Slug, null, Guid.NewGuid()));
            }

            return list.AsReadOnly();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoryFailedActionReducer : Reducer<StoryState, GetStoryFailedAction>
    {
        public override StoryState Reduce(StoryState state, GetStoryFailedAction action) =>
            new StoryState(ModelStatus.Failed(action.Error));
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class ComposeReplyActionReducer : Reducer<StoryState, ComposeReplyAction>
    {
        /// <inheritdoc cref="Reducer{TState,TAction}.Reduce" />
        public override StoryState Reduce(StoryState state, ComposeReplyAction action)
        {
            //Debug.WriteLine($"ComposeReplyActionReducer.Reduce status: {state.Status.State}");
            return new StoryState(state.Status)
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
        }

        private static IReadOnlyCollection<CommentBase> AddComposingComment(
            IEnumerable<CommentBase> comments,
            ComposeReplyAction action)
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

                        if (action.ParentId.Equals(comment.Id))
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
    // ReSharper disable once UnusedMember.Global
    public sealed class SaveReplyActionReducer : Reducer<StoryState, SaveReplyAction>
    {
        public override StoryState Reduce(StoryState state, SaveReplyAction action) =>
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
            SaveReplyAction action)
        {
            IReadOnlyCollection<CommentBase> Project(IEnumerable<CommentBase> collection)
            {
                var result = new List<CommentBase>();

                foreach (var child in collection)
                {
                    if (child is Comment comment)
                    {
                        result.Add(new Comment(comment.StorySlug, comment.Parent)
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
    // ReSharper disable once UnusedMember.Global
    public sealed class ReplyPublishedActionReducer : Reducer<StoryState, ReplyPublishedAction>
    {
        public override StoryState Reduce(StoryState state, ReplyPublishedAction action)
        {
            //Debug.WriteLine("ReplyPublishedActionReducer.Reduce: status: Success");
            return new StoryState(ModelStatus.Success)
            {
                Slug = state.Slug,
                Title = state.Title,
                Author = state.Author,
                Content = state.Content,
                Published = state.Published,
                IsCommentsClosed = state.IsCommentsClosed,
                CommentsCount = state.CommentsCount + 1,
                Comments = ReplaceSavingComment(state.Comments, action)
            };
        }

        private static IReadOnlyCollection<CommentBase> ReplaceSavingComment(
            IEnumerable<CommentBase> comments,
            ReplyPublishedAction action)
        {
            IReadOnlyCollection<CommentBase> Project(IEnumerable<CommentBase> collection)
            {
                var result = new List<CommentBase>();

                foreach (var child in collection)
                {
                    if (child is Comment comment)
                    {
                        result.Add(new Comment(comment.StorySlug, comment.Parent)
                        {
                            Id = comment.Id,
                            Content = comment.Content,
                            Author = comment.Author,
                            Published = comment.Published,
                            Comments = Project(comment.Comments)
                        });

                        continue;
                    }

                    if (child is SavingComment saving && action.Reference == saving.Reference)
                    {
                        //Debug.WriteLine($"[ReplyPublishedActionReducer.Reduce] Replacing comment for: \'{action.StorySlug}\' parent: {action.ParentId} ref: {action.Reference}");
                        result.Add(new Comment(saving.StorySlug, saving.Parent)
                        {
                            Id = action.Id,
                            Author = action.Author,
                            Content = action.Content,
                            Published = action.Published
                        });
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