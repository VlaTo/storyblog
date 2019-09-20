using System;
using System.Collections.Generic;
using StoryBlog.Web.Services.Shared.MarkDown.Elements;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public sealed class HtmlContentComposer : MarkDownDocumentVisitor
    {
        private readonly IDictionary<Type, IHtmlContentComposerDecorator> decorators;
        private readonly Stack<HtmlTag> scopes;
        private readonly HtmlTagFactory factory;
        private readonly HtmlWriter writer;

        public HtmlContentComposer(HtmlTagFactory factory, HtmlWriter writer)
        {
            if (null == factory)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            decorators = new Dictionary<Type, IHtmlContentComposerDecorator>();
            scopes = new Stack<HtmlTag>();

            this.factory = factory;
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
            var htmlTag = scopes.Peek();

            if (null == htmlTag)
            {
                throw new Exception();
            }

            writer.WriteContent(textElement.Text);
        }

        protected override void VisitDocument(MarkDownDocument document)
        {
            var decorator = GetDecoratorFor<MarkDownDocument>();
            var htmlTag = factory.CreateDiv();

            decorator.Apply(htmlTag, document);

            htmlTag.WriteOpen(writer);

            base.VisitDocument(document);

            htmlTag.WriteClose(writer);
        }

        protected override void VisitHeadingElement(MarkDownHeadingElement headingElement)
        {
            var decorator = GetDecoratorFor<MarkDownHeadingElement>();
            var level = Math.Max(1, Math.Min(headingElement.Level, 6));
            var tag = factory.CreateHeading(level);

            decorator.Apply(tag, headingElement);

            tag.WriteOpen(writer);

            base.VisitHeadingElement(headingElement);

            tag.WriteClose(writer);
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