using System;
using System.Collections.Generic;
using StoryBlog.Web.Services.Shared.MarkDown.Elements;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public abstract class MarkDownDocumentVisitor : IVisitor<MarkDownDocument>
    {
        public void Visit(MarkDownDocument obj)
        {
            if (null == obj)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            VisitDocument(obj);
        }

        protected abstract void VisitTextElement(MarkDownTextElement textElement);

        protected virtual void VisitDocument(MarkDownDocument document)
        {
            VisitChildren(document.Children);
        }

        protected virtual void VisitHeadingElement(MarkDownHeadingElement headingElement)
        {
            VisitChildren(headingElement.Children);
        }

        private void VisitChildren(IList<MarkDownElement> children)
        {
            foreach (var child in children)
            {
                if (child is MarkDownTextElement text)
                {
                    VisitTextElement(text);
                }
                else if (child is MarkDownHeadingElement heading)
                {
                    VisitHeadingElement(heading);
                }
            }
        }
    }
}