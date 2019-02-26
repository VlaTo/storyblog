using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    public sealed class Story : StoryBase
    {
        public IList<Comment> Comments
        {
            get;
        }

        public Story(long id)
            : base(id)
        {
            Comments = new List<Comment>();
        }
    }
}