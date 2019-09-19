using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.MarkDown.Elements
{
    public class MarkDownNode : MarkDownElement
    {
        public IList<MarkDownElement> Children
        {
            get;
        }

        public MarkDownNode()
            : this(null)
        {
        }

        protected MarkDownNode(IList<MarkDownElement> children)
        {
            Children = children ?? new List<MarkDownElement>();
        }
    }
}