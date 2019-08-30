using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Interop.Markups.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    public enum BulletingBoardBlockType
    {
        /// <summary>
        /// 
        /// </summary>
        Document,

        /// <summary>
        /// 
        /// </summary>
        Em,

        /// <summary>
        /// 
        /// </summary>
        Strong,

        /// <summary>
        /// 
        /// </summary>
        Underline,

        /// <summary>
        /// 
        /// </summary>
        Hyperlink,
        Marked,
        Cite,
        Dfn,
        Abbr,
        Italic
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BulletingBoardBlock : BulletingBoardElement
    {
        /// <summary>
        /// 
        /// </summary>
        public BulletingBoardBlockType BlockType
        {
            get;
        }

        public string Argument
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsClosing
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<BulletingBoardElement> Elements
        {
            get;
        }

        protected BulletingBoardBlock(BulletingBoardBlockType type, bool closing)
        {
            BlockType = type;
            IsClosing = closing;
            Elements = new List<BulletingBoardElement>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static BulletingBoardBlock ReadBlockTag(string text, ref int start, int end)
        {
            if (Terminals.OpenBracket != text[start])
            {
                return null;
            }

            start++;

            var closing = false;

            if (start < end && Terminals.Slash == text[start])
            {
                closing = true;
                start++;
            }

            var position = start;

            while (position < end)
            {
                var ch = text[position];

                if (Char.IsLetterOrDigit(ch) || '-' == ch)
                {
                    position++;
                    continue;
                }

                break;
            }

            var count = position - start;

            if (0 >= count)
            {
                return null;
            }

            var block = CreateBlockFromText(text.Substring(start, count), closing);

            if (false == closing)
            {
                block.Argument = ReadArgument(text, ref position, end);
            }

            start = position;

            return block;
        }

        private static string ReadArgument(string text, ref int start, int end)
        {
            if (start >= end || '=' != text[start])
            {
                return null;
            }

            var current = start + 1;

            if (Terminals.Quote == text[current])
            {
                if (++current >= end)
                {
                    throw new BulletingBoardMarkupException();
                }

                var last = text.IndexOf(text[current - 1], current, end - current);

                if (last > current && last <= end)
                {
                    start = last + 1;

                    return text.Substring(current, last - current);
                }

                throw new BulletingBoardMarkupException();
            }

            var position = text.IndexOf(Terminals.CloseBracket, current, end - current);

            if (position <= current || position > end)
            {
                throw new BulletingBoardMarkupException();
            }

            start = position;

            return text.Substring(current, position - current);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        public bool MatchOpen(BulletingBoardBlock block)
        {
            if (null == block)
            {
                throw new BulletingBoardMarkupException();
            }

            return IsClosing && BlockType == block.BlockType && false == block.IsClosing;
        }

        private static BulletingBoardBlock CreateBlockFromText(string tag, bool closing)
        {
            if (String.IsNullOrEmpty(tag))
            {
                throw new ArgumentNullException(nameof(tag));
            }

            switch (tag)
            {
                case "em":
                {
                    return new BulletingBoardEmphasized(closing);
                }

                case "strong":
                {
                    return new BulletingBoardStrong(closing);
                }

                case "mark":
                {
                    return new BulletingBoardMarked(closing);
                }

                case "cite":
                {
                    return new BulletingBoardCite(closing);
                }

                case "italic":
                {
                    return new BulletingBoardItalic(closing);
                }

                case "underline":
                {
                    return new BulletingBoardUnderline(closing);
                }

                case "dnf":
                {
                    return new BulletingBoardTerm(closing);
                }

                case "abbr":
                {
                    return new BulletingBoardAbbr(closing);
                }

                case "link":
                {
                    return new BulletingBoardHyperlink(closing);
                }
            }

            throw new BulletingBoardMarkupException();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardStrong : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardStrong(bool closing)
            : base(BulletingBoardBlockType.Strong, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardEmphasized : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardEmphasized(bool closing)
            : base(BulletingBoardBlockType.Em, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardMarked : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardMarked(bool closing)
            : base(BulletingBoardBlockType.Marked, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardCite : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardCite(bool closing)
            : base(BulletingBoardBlockType.Cite, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardTerm : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardTerm(bool closing)
            : base(BulletingBoardBlockType.Dfn, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardAbbr : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardAbbr(bool closing)
            : base(BulletingBoardBlockType.Abbr, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardItalic : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardItalic(bool closing)
            : base(BulletingBoardBlockType.Italic, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardUnderline : BulletingBoardBlock
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="closing"></param>
        public BulletingBoardUnderline(bool closing)
            : base(BulletingBoardBlockType.Underline, closing)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardHyperlink : BulletingBoardBlock
    {
        public BulletingBoardHyperlink(bool closing)
            : base(BulletingBoardBlockType.Hyperlink, closing)
        {
        }
    }
}
