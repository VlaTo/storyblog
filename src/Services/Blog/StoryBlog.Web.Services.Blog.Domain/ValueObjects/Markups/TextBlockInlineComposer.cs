using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Documents;

namespace LibraProgramming.Windows.UI.Xaml.Core.Markups
{
    /// <summary>
    /// 
    /// </summary>
    internal class MarkupNodeVisitor : MarkupVisitor
    {
        public override void Visit(MarkupNode node)
        {
            VisitNode(node);
        }

        protected virtual void VisitNode(MarkupNode node)
        {
            var span = node as SpanNode;

            if (null != span)
            {
                VisitSpan(span);
                return;
            }

            var run = node as RunNode;

            if (null != run)
            {
                VisitRun(run);
                return;
            }
        }

        protected virtual void VisitSpan(SpanNode span)
        {
            foreach (var node in span.Inlines)
            {
                VisitNode(node);
            }
        }

        protected virtual void VisitRun(RunNode run)
        {
        }
    }

    internal sealed class TextBlockInlineComposer : MarkupNodeVisitor
    {
        private readonly InlineCollection inlines;
        private readonly Stack<InlineCollection> collections;

        public TextBlockInlineComposer(InlineCollection inlines)
        {
            collections = new Stack<InlineCollection>();
            this.inlines = inlines;
        }

        public override void Visit(MarkupNode node)
        {
            collections.Push(inlines);

            base.Visit(node);

            collections.Pop();
        }

        protected override void VisitSpan(SpanNode span)
        {
            var collection = collections.Peek();
            var inline = CreateInline(span);

            collection.Add(inline);
            collections.Push(inline.Inlines);

            base.VisitSpan(span);

            collections.Pop();
        }

        protected override void VisitRun(RunNode run)
        {
            var collection = collections.Peek();

            collection.Add(
                new Run
                {
                    Text = run.Text
                });

            base.VisitRun(run);
        }

        private static Span CreateInline(SpanNode span)
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
        }
    }
}