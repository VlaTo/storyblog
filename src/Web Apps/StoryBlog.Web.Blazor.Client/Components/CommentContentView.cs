using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using StoryBlog.Web.Services.Shared.BBCode.Composing;
using StoryBlog.Web.Services.Shared.BBCode.Nodes;
using StoryBlog.Web.Services.Shared.BBCode.Parsing;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Client.Components
{
    public sealed class CommentContentView : ComponentBase
    {
        private readonly BulletingBoardDocument document;
        private string content;

        public CommentContentView()
        {
            document = new BulletingBoardDocument();
        }

        [Parameter]
        public string Content
        {
            get => content;
            set
            {
                if (String.Equals(content, value))
                {
                    return;
                }

                content = value;
                document.Parse(value);
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var composer = new RenderTreeHtmlComposer(builder);
            composer.Visit(document);
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class RenderTreeHtmlComposer : BulletingBoardDocumentVisitor
        {
            private static readonly Dictionary<BulletingBoardBlockType, string> tags;
            private readonly RenderTreeBuilder builder;
            private int sequence;

            public RenderTreeHtmlComposer(RenderTreeBuilder builder)
            {
                this.builder = builder;
            }

            static RenderTreeHtmlComposer()
            {
                tags = new Dictionary<BulletingBoardBlockType, string>
                {
                    {BulletingBoardBlockType.Em, "em"},
                    {BulletingBoardBlockType.Abbr, "abbr"},
                    {BulletingBoardBlockType.Cite, "cite"},
                    {BulletingBoardBlockType.Dfn, "dfn"},
                    {BulletingBoardBlockType.Italic, "i"},
                    {BulletingBoardBlockType.Marked, "mark"},
                    {BulletingBoardBlockType.Hyperlink, "a"},
                    {BulletingBoardBlockType.Strong, "strong"},
                    {BulletingBoardBlockType.Underline, "u"}
                };
            }

            public override void Visit(BulletingBoardDocument obj)
            {
                if (null == obj)
                {
                    throw new ArgumentNullException(nameof(obj));
                }

                sequence = 0;

                base.Visit(obj);
            }

            protected override void RenderStrong(BulletingBoardStrong strong)
            {
                builder.OpenElement(sequence++, GetElementName(strong.BlockType));
                base.RenderStrong(strong);
                builder.CloseElement();
            }

            protected override void RenderEmphasized(BulletingBoardEmphasized em)
            {
                builder.OpenElement(sequence++, GetElementName(em.BlockType));
                base.RenderEmphasized(em);
                builder.CloseElement();
            }

            protected override void RenderCite(BulletingBoardCite cite)
            {
                builder.OpenElement(sequence++, GetElementName(cite.BlockType));
                base.RenderCite(cite);
                builder.CloseElement();
            }

            protected override void RenderAbbr(BulletingBoardAbbr abbr)
            {
                builder.OpenElement(sequence++, GetElementName(abbr.BlockType));
                builder.AddAttribute(sequence++, "title", abbr.Argument);
                base.RenderAbbr(abbr);
                builder.CloseElement();
            }

            protected override void RenderTerm(BulletingBoardTerm term)
            {
                builder.OpenElement(sequence++, GetElementName(term.BlockType));
                base.RenderTerm(term);
                builder.CloseElement();
            }

            protected override void RenderItalic(BulletingBoardItalic italic)
            {
                builder.OpenElement(sequence++, GetElementName(italic.BlockType));
                base.RenderItalic(italic);
                builder.CloseElement();
            }

            protected override void RenderUnderline(BulletingBoardUnderline block)
            {
                builder.OpenElement(sequence++, GetElementName(block.BlockType));
                base.RenderUnderline(block);
                builder.CloseElement();
            }

            protected override void RenderHyperlink(BulletingBoardHyperlink link)
            {
                builder.OpenElement(sequence++, GetElementName(link.BlockType));
                builder.AddAttribute(sequence++, "href", link.Argument);
                base.RenderHyperlink(link);
                builder.CloseElement();
            }

            protected override void RenderText(BulletingBoardText inline)
            {
                builder.AddContent(sequence++, inline.Text);
            }

            private static string GetElementName(BulletingBoardBlockType blockType) =>
                tags.TryGetValue(blockType, out var tag) ? tag : "undefined";
        }
    }
}