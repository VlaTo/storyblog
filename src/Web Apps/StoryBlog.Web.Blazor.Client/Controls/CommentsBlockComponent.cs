using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Blazor.Client.Store.Models;

namespace StoryBlog.Web.Blazor.Client.Controls
{
    public class CommentsBlockComponent : ComponentBase
    {
        private IReadOnlyCollection<CommentBase> comments;

        [Parameter]
        public int Level
        {
            get;
            set;
        }

        [Parameter]
        public IReadOnlyCollection<CommentBase> Comments
        {
            get
            {
                if (null == comments)
                {
                    comments = new CommentBase[0];
                }

                return comments;
            }
            set => comments = value;
        }

        protected static ComposeComment GetReplyCompose(IReadOnlyCollection<CommentBase> comments)
        {
            if (0 == comments.Count)
            {
                return null;
            }

            return comments.OfType<ComposeComment>().FirstOrDefault();
        }
    }
}