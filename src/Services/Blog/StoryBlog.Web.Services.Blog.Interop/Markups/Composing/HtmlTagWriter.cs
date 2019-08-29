using System;
using System.Collections.Generic;
using System.Text;
using StoryBlog.Web.Services.Blog.Interop.Markups.Nodes;
using StoryBlog.Web.Services.Blog.Interop.Extensions;

namespace StoryBlog.Web.Services.Blog.Interop.Markups.Composing
{
    internal sealed class HtmlTagWriter : IDisposable
    {
        private StringBuilder stringBuilder;
        private Stack<TagCloseToken> scopes;
        private static readonly Dictionary<BulletingBoardBlockType, string> tags;
        private bool disposed;

        public HtmlTagWriter(StringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;
            scopes = new Stack<TagCloseToken>();
        }

        static HtmlTagWriter()
        {
            tags = new Dictionary<BulletingBoardBlockType, string>
            {
                {BulletingBoardBlockType.Hyperlink, "a"},
                {BulletingBoardBlockType.Strong, "b"},
                {BulletingBoardBlockType.Underline, "i"}
            };
        }

        public IDisposable OpenTag(BulletingBoardBlockType blockType, string argument = null)
        {
            var token = new TagCloseToken(this, blockType);

            scopes.Push(token);
            WriteTag(blockType, argument, false);

            return token;
        }

        public void WriteText(string text)
        {
            stringBuilder.Append(text);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void WriteTag(BulletingBoardBlockType blockType, string argument, bool closing)
        {
            stringBuilder
                .Append('<')
                .AppendIf(Terminals.Slash, closing)
                .Append(GetTag(blockType));

            if (false == closing && false == String.IsNullOrEmpty(argument))
            {
                switch (blockType)
                {
                    case BulletingBoardBlockType.Hyperlink:
                    {
                        stringBuilder
                            .Append(" href=")
                            .Append(Terminals.Quote)
                            .Append(argument)
                            .Append(Terminals.Quote);
                        break;
                    }

                    default:
                    {
                        break;
                    }
                }
            }

            stringBuilder.Append('>');
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    stringBuilder = null;
                    scopes = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }

        private static string GetTag(BulletingBoardBlockType blockType) =>
            tags.TryGetValue(blockType, out var tag) ? tag : "undefined";

        /// <summary>
        /// 
        /// </summary>
        private sealed class TagCloseToken : IDisposable
        {
            private readonly HtmlTagWriter writer;
            private readonly BulletingBoardBlockType blockType;

            public TagCloseToken(HtmlTagWriter writer, BulletingBoardBlockType blockType)
            {
                this.writer = writer;
                this.blockType = blockType;
            }

            public void Dispose()
            {
                writer.WriteTag(blockType, null, true);
            }
        }
    }
}