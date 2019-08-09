using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    public class Story : StoryBase
    {
        public ICollection<Comment> Comments
        {
            get;
        }

        public Story()
        {
            Comments = new Collection<Comment>();
        }
    }
}