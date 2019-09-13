namespace StoryBlog.Web.Client.Store.Helpers
{
    /*internal static class Comments
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="comments"></param>
        public static void CreateCommentsTree(
            ICollection<Comment> collection,
            IReadOnlyCollection<Web.Services.Blog.Interop.Models.CommentModel> comments)
        {
            foreach (var comment in comments)
            {
                if (comment.Parent.HasValue)
                {
                    continue;
                }

                var model = new Comment(null)
                {
                    Content = comment.Content,
                    Author = new Author
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
            Comment parent,
            IReadOnlyCollection<Web.Services.Blog.Interop.Models.CommentModel> comments)
        {
            foreach (var comment in comments)
            {
                if (false == comment.Parent.HasValue || comment.Parent.Value != parent.Id)
                {
                    continue;
                }

                var model = new Comment(parent, comment.Id)
                {
                    Content = comment.Content,
                    Author = new Author
                    {
                        Name = "(none)" //comment.Author.Name
                    },
                    Published = comment.Modified.GetValueOrDefault(comment.Created)
                };

                parent.Comments.Add(model);

                CreateNestedComments(model, comments);
            }
        }
    }*/
}