using StoryBlog.Web.Blazor.Client.Store.Models;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Helpers
{
    internal static class Comments
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comments"></param>
        public static void CreateCommentsTree(
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
                    Author = new AuthorModel
                    {
                        Name = "(none)" //comment.Author.Name
                    },
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
                    Author = new AuthorModel
                    {
                        Name = "(none)" //comment.Author.Name
                    },
                    Published = comment.Modified.GetValueOrDefault(comment.Created)
                };

                parent.Comments.Add(model);

                CreateNestedComments(model, comments);
            }
        }
    }
}