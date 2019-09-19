using StoryBlog.Web.Services.Shared.BBCode.Nodes;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.BBCode.Parsing
{
    public sealed class BulletingBoardDocument : BulletingBoardBlock
    {
        public BulletingBoardDocument()
            : base(BulletingBoardBlockType.Document, false)
        {
        }

        public void Parse(string text)
        {
            var blocks = new Stack<BulletingBoardBlock>();

            Elements.Clear();
            blocks.Push(this);

            Parse(text, 0, text.Length, blocks);
        }
        
        private static void Parse(string text, int start, int end, Stack<BulletingBoardBlock> blocks)
        {
            const char nosym = '\0';
            var lineStart = start;

            while (lineStart < end)
            {
                var nonSpacePosition = lineStart;
                var nonSpaceSym = nosym;

                while (nonSpacePosition < end)
                {
                    var ch = text[nonSpacePosition];

                    if (Terminals.Cr == ch || Terminals.Lf == ch)
                    {
                        break;
                    }

                    if (false == Char.IsWhiteSpace(ch))
                    {
                        nonSpaceSym = ch;
                        break;
                    }

                    nonSpacePosition++;
                }

                var lineEnd = FindLineEnd(text, lineStart, end, out var nextLineStart);

                while (lineStart < lineEnd)
                {
                    if (nosym == nonSpaceSym)
                    {
                        nonSpaceSym = text[lineStart];
                    }

                    if (Terminals.CloseBracket == nonSpaceSym)
                    {
                        nonSpaceSym = nosym;

                        if (lineStart < lineEnd && Char.IsWhiteSpace(text[lineStart]))
                        {
                            lineStart++;
                        }
                        else
                        {
                            lineStart++;
                        }
                    }
                    else if (Terminals.OpenBracket == nonSpaceSym)
                    {
                        var block = ReadBlockTag(text, ref lineStart, lineEnd);

                        if (null == block)
                        {
                            continue;
                        }

                        if (block.IsClosing)
                        {
                            if (block.MatchOpen(blocks.Peek()))
                            {
                                blocks.Pop();
                            }
                            else
                            {
                                throw new BulletingBoardMarkupException();
                            }
                        }
                        else
                        {
                            var parent = blocks.Peek();

                            parent.Elements.Add(block);
                            blocks.Push(block);
                        }

                        nonSpaceSym = nosym;
                    }
                    else
                    {
                        var paragraph = BulletingBoardText.Read(text, ref lineStart, lineEnd);

                        if (null != paragraph)
                        {
                            var block = blocks.Peek();
                            block.Elements.Add(paragraph);
                        }

                        nonSpaceSym = nosym;
                    }
                }

                lineStart = nextLineStart;
            }
        }
    }
}