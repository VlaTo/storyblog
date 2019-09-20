using StoryBlog.Web.Services.Shared.BBCode.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StoryBlog.Web.Services.Shared.BBCode.Composing
{
    public sealed class HtmlWriter : IDisposable
    {
        private StringWriter writer;
        private Stack<TagCloseToken> scopes;
        private static readonly Dictionary<BulletingBoardBlockType, string> tags;
        private bool disposed;

        public HtmlWriter(StringWriter writer)
        {
            this.writer = writer;
            scopes = new Stack<TagCloseToken>();
        }

        static HtmlWriter()
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
            writer.Write(text);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void WriteTag(BulletingBoardBlockType blockType, string argument, bool closing)
        {
            writer.Write('<');

            if (closing)
            {
                writer.Write(Terminals.Slash);
            }

            writer.Write(GetTag(blockType));

            /*writer
                .Append('<')
                .AppendIf(Terminals.Slash, closing)
                .Append(GetTag(blockType));*/

            if (false == closing && false == String.IsNullOrEmpty(argument))
            {
                switch (blockType)
                {
                    case BulletingBoardBlockType.Hyperlink:
                    {
                        writer.Write(" href=");
                        writer.Write(Terminals.Quote);
                        writer.Write(argument);
                        writer.Write(Terminals.Quote);

                        /*writer
                            .Append(" href=")
                            .Append(Terminals.Quote)
                            .Append(argument)
                            .Append(Terminals.Quote);*/

                        break;
                    }

                    default:
                    {
                        break;
                    }
                }
            }

            writer.Write('>');
            //writer.Append('>');
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
                    writer = null;
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
            private readonly HtmlWriter writer;
            private readonly BulletingBoardBlockType blockType;

            public TagCloseToken(HtmlWriter writer, BulletingBoardBlockType blockType)
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