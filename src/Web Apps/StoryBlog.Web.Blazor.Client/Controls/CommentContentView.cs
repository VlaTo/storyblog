using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using StoryBlog.Web.Services.Blog.Interop.Markups.Composing;
using StoryBlog.Web.Services.Blog.Interop.Markups.Nodes;
using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;

namespace StoryBlog.Web.Blazor.Client.Controls
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
            //base.BuildRenderTree(builder);
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
                    {BulletingBoardBlockType.Hyperlink, "a"},
                    {BulletingBoardBlockType.Strong, "b"},
                    {BulletingBoardBlockType.Underline, "i"}
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

            protected override void RenderBold(BulletingBoardBold block)
            {
                builder.OpenElement(sequence++, GetElementName(block.BlockType));
                base.RenderBold(block);
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