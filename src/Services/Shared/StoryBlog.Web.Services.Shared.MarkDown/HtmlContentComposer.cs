using System;
using System.Collections.Generic;
using StoryBlog.Web.Services.Shared.MarkDown.Elements;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public sealed class HtmlContentComposer : MarkDownDocumentVisitor
    {
        private readonly IDictionary<Type, IHtmlContentComposerDecorator> decorators;
        private readonly HtmlTagWriter writer;

        public HtmlContentComposer(HtmlTagWriter writer)
        {
            if (null == writer)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            decorators = new Dictionary<Type, IHtmlContentComposerDecorator>();

            this.writer = writer;
        }

        public void AddDecorator<TElement>(IHtmlContentComposerDecorator decorator)
            where TElement : MarkDownElement
        {
            if (null == decorator)
            {
                throw new ArgumentNullException(nameof(decorator));
            }

            if (false == decorators.TryGetValue(typeof(TElement), out var existingDecorator))
            {
                decorators[typeof(TElement)] = decorator;
                return;
            }

            if (existingDecorator is CompositeHtmlContentDecorator compositeDecorator)
            {
                compositeDecorator.Decorators.Add(decorator);
                return;
            }

            decorators[typeof(TElement)] = new CompositeHtmlContentDecorator(existingDecorator, decorator);
        }

        protected override void VisitTextElement(MarkDownTextElement textElement)
        {
            writer.AddContent(textElement.Text);
        }

        protected override void VisitDocument(MarkDownDocument document)
        {
            var decorator = GetDecoratorFor<MarkDownDocument>();
            var tag = writer.OpenElement("div");

            decorator.Apply(tag, document);

            tag.WriteOpen();

            base.VisitDocument(document);

            tag.WriteClose();
        }

        protected override void VisitHeadingElement(MarkDownHeadingElement headingElement)
        {
            var decorator = GetDecoratorFor<MarkDownHeadingElement>();
            var level = Math.Max(1, Math.Min(headingElement.Level, 6));
            var tag = writer.OpenElement($"h{level}");

            decorator.Apply(tag, headingElement);

            tag.WriteOpen();

            base.VisitHeadingElement(headingElement);

            tag.WriteClose();
        }

        private IHtmlContentComposerDecorator GetDecoratorFor<TElement>()
            where TElement : MarkDownElement =>
            decorators.TryGetValue(typeof(TElement), out var decorator)
                ? decorator
                : EmptyHtmlContentDecorator.Instance;

        /// <summary>
        /// 
        /// </summary>
        private class CompositeHtmlContentDecorator : IHtmlContentComposerDecorator
        {
            public IList<IHtmlContentComposerDecorator> Decorators
            {
                get;
            }

            public CompositeHtmlContentDecorator(params IHtmlContentComposerDecorator[] decorators)
            {
                Decorators = new List<IHtmlContentComposerDecorator>(decorators);
            }

            public void Apply(HtmlTag tag, MarkDownElement element)
            {
                foreach (var decorator in Decorators)
                {
                    decorator.Apply(tag, element);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class EmptyHtmlContentDecorator : IHtmlContentComposerDecorator
        {
            public static EmptyHtmlContentDecorator Instance
            {
                get;
            }

            private EmptyHtmlContentDecorator()
            {
            }

            static EmptyHtmlContentDecorator()
            {
                Instance = new EmptyHtmlContentDecorator();
            }

            public void Apply(HtmlTag tag, MarkDownElement element)
            {
            }
        }
    }
}