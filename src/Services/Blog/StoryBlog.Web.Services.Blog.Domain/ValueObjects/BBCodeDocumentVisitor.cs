using System;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Domain.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BBCodeDocumentVisitor : NodeVisitor
    {
        private readonly StringBuilder content;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public BBCodeDocumentVisitor(StringBuilder content)
        {
            if (null == content)
            {
                throw new ArgumentNullException(nameof(content));
            }

            this.content = content;
        }

        protected override void VisitDocument(BBCodeDocument document)
        {
            foreach (var node in document)
            {
                Visit(node);
            }
        }

        protected override void VisitNode(Node node)
        {
            throw new NotImplementedException();
        }
    }
}