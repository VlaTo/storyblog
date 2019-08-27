using System;

namespace StoryBlog.Web.Services.Blog.Domain.ValueObjects
{
    public abstract class NodeVisitor
    {
        public virtual void Visit(Node node)
        {
            if (null == node)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node is BBCodeDocument document)
            {
                VisitDocument(document);
            }
            else
            {
                VisitNode(node);
            }
        }

        protected abstract void VisitDocument(BBCodeDocument document);

        protected abstract void VisitNode(Node node);
    }
}