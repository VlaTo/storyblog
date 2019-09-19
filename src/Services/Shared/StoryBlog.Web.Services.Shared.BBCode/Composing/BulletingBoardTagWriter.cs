using StoryBlog.Web.Services.Shared.BBCode.Nodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Services.Shared.BBCode.Composing
{
    internal class BulletingBoardTagWriter
    {
        private readonly StringBuilder stringBuilder;
        private readonly Stack<TagCloseToken> scopes;
        private static readonly Dictionary<BulletingBoardBlockType, string> tags;

        public BulletingBoardTagWriter(StringBuilder stringBuilder)
        {
            this.stringBuilder = stringBuilder;
            scopes = new Stack<TagCloseToken>();
        }

        static BulletingBoardTagWriter()
        {
            tags = new Dictionary<BulletingBoardBlockType, string>
            {
                {BulletingBoardBlockType.Strong, "strong"},
                {BulletingBoardBlockType.Underline, "underline"}
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

        private void WriteTag(BulletingBoardBlockType blockType, string argument, bool closing)
        {
            stringBuilder
                .Append(Terminals.OpenBracket)
                .AppendIf(Terminals.Slash, closing)
                .Append(GetTag(blockType));

            if (false == closing && false == String.IsNullOrEmpty(argument))
            {
                stringBuilder
                    .Append('=')
                    .Append(Terminals.Quote)
                    .Append(argument)
                    .Append(Terminals.Quote);
            }

            stringBuilder.Append(Terminals.CloseBracket);
        }

        private static string GetTag(BulletingBoardBlockType blockType) =>
            tags.TryGetValue(blockType, out var tag) ? tag : "undefined";

        /// <summary>
        /// 
        /// </summary>
        private sealed class TagCloseToken : IDisposable
        {
            private readonly BulletingBoardTagWriter writer;
            private readonly BulletingBoardBlockType blockType;

            public TagCloseToken(BulletingBoardTagWriter writer, BulletingBoardBlockType blockType)
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