using System.Collections.Generic;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Models;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryActionReducer : Reducer<StoryState, GetStoryAction>
    {
        public override StoryState Reduce(StoryState state, GetStoryAction action) => new StoryState(ModelStatus.Loading);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStorySuccessActionReducer : Reducer<StoryState, GetStorySuccessAction>
    {
        public override StoryState Reduce(StoryState state, GetStorySuccessAction action)
        {
            var result = new StoryState(ModelStatus.Success)
            {
                Title = action.Title,
                Slug = action.Slug,
                Content = action.Content,
                AllCommentsCount = action.Comments.Count
            };

            CreateComments(result.Comments, action.Comments);

            return result;
        }

        private static void CreateComments(
            ICollection<CommentModel> collection, 
            IReadOnlyCollection<Web.Services.Blog.Interop.Models.CommentModel> comments)
        {
            foreach (var comment in comments)
            {
                if (comment.Parent.HasValue)
                {
                    continue;
                }

                var model = new CommentModel(null, comment.Id)
                {
                    Content = comment.Content,
                    Published = comment.Modified.GetValueOrDefault(comment.Created)
                };

                collection.Add(model);

                CreateNestedComments(model, comments);
            }
        }

        private static void CreateNestedComments(
            CommentModel parent, 
            IReadOnlyCollection<Web.Services.Blog.Interop.Models.CommentModel> comments)
        {
            foreach (var comment in comments)
            {
                if (false == comment.Parent.HasValue || comment.Parent.Value != parent.Id)
                {
                    continue;
                }

                var model = new CommentModel(parent, comment.Id)
                {
                    Content = comment.Content,
                    Published = comment.Modified.GetValueOrDefault(comment.Created)
                };

                parent.Comments.Add(model);

                CreateNestedComments(model, comments);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryFailedActionReducer : Reducer<StoryState, GetStoryFailedAction>
    {
        public override StoryState Reduce(StoryState state, GetStoryFailedAction action) => new StoryState(ModelStatus.Failed(action.Error));
    }
}