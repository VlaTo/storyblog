using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Models
{
    public sealed class CommentModel
    {
        public long Id
        {
            get;
        }

        public CommentModel Parent
        {
            get;
        }

        public string Content
        {
            get;
            set;
        }

        public DateTime Published
        {
            get;
            set;
        }

        public IList<CommentModel> Comments
        {
            get;
        }

        public CommentModel(CommentModel parent, long id)
        {
            Parent = parent;
            Id = id;
            Comments = new List<CommentModel>();
        }
    }
}