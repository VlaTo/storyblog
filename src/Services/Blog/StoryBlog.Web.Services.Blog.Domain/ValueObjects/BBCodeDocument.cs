using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Domain.ValueObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class BBCodeDocument : Node, IEnumerable<Node>
    {
        /// <summary>
        /// 
        /// </summary>
        public BBCodeDocument()
        {
        }

        public IEnumerator<Node> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var content = new StringBuilder();
            var visitor = new BBCodeDocumentVisitor(content);

            visitor.Visit(this);

            return content.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static BBCodeDocument Parse(string source)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new BBCodeDocument();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static bool TryParse(string source, out BBCodeDocument document)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            document = new BBCodeDocument();

            return true;
        }
    }
}