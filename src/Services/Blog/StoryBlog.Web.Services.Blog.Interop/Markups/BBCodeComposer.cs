using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    /// <summary>
    /// 
    /// </summary>
    public class MarkupNodeVisitor : MarkupVisitor
    {
        public override void Visit(MarkupNode node)
        {
            VisitNode(node);
        }

        protected virtual void VisitNode(MarkupNode node)
        {
            var span = node as TagNode;

            if (null != span)
            {
                VisitSpan(span);
                return;
            }

            var run = node as TextNode;

            if (null != run)
            {
                VisitRun(run);
                return;
            }
        }

        protected virtual void VisitSpan(TagNode tag)
        {
            foreach (var node in tag.Inlines)
            {
                VisitNode(node);
            }
        }

        protected virtual void VisitRun(TextNode text)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class BBCodeComposer : MarkupNodeVisitor
    {
        private readonly StringBuilder builder;
        //private readonly Stack<InlineCollection> collections;

        public BBCodeComposer(StringBuilder builder)
        {
            this.builder = builder;
            //collections = new Stack<InlineCollection>();
            //this.inlines = inlines;
        }

        public override void Visit(MarkupNode node)
        {
            //collections.Push(inlines);

            base.Visit(node);

            //collections.Pop();
        }

        protected override void VisitSpan(TagNode tag)
        {
            //var collection = collections.Peek();
            //var inline = CreateInline(span);

            //collection.Add(inline);
            //collections.Push(inline.Inlines);

            base.VisitSpan(tag);

            //collections.Pop();
        }

        protected override void VisitRun(TextNode text)
        {
            //var collection = collections.Peek();

            /*collection.Add(
                new Run
                {
                    Text = run.Text
                });*/

            base.VisitRun(text);
        }

        /*private static Span CreateInline(SpanNode span)
        {
            if (null == span.Tag)
            {
                return new Span();
            }

            switch (span.Tag)
            {
                case "b":
                {
                    return new Bold();
                }

                case "i":
                {
                    return new Italic();
                }

                case "u":
                {
                    return new Underline();
                }
            }

            throw new InvalidOperationException();
        }*/
    }
}