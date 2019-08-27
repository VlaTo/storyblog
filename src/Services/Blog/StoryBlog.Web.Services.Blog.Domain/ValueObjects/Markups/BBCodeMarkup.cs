using System;
using System.Collections.Generic;
using System.Text;

namespace LibraProgramming.Windows.UI.Xaml.Core.Markups
{
    internal class BBCodeNode : MarkupNode
    {

    }

    /// <summary>
    /// 
    /// </summary>
    internal class SpanNode : MarkupNode
    {
        public string Tag
        {
            get;
            set;
        }

        public IList<MarkupNode> Inlines
        {
            get;
        }

        public SpanNode()
        {
            Inlines = new List<MarkupNode>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class RunNode : MarkupNode
    {
        public string Text
        {
            get;
        }

        public RunNode(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class BBCodeMarkup : IDisposable
    {
        private bool disposed;

        public SpanNode Parse(string text)
        {
            EnsureNotDisposed();

            var state = ParseTextState.Unknown;

            using (var reader = new StringReader(text))
            {
                if (false == reader.Advance())
                {
                    return null;
                }

                var buffer = new StringBuilder();
                var nodes = new Stack<SpanNode>();

                while (true)
                {
                    switch (state)
                    {
                        case ParseTextState.Unknown:
                        {
                            var token = ParseText(reader);

                            if (null == token)
                            {
                                break;
                            }

                            var term = token as TermToken;

                            if (null != term)
                            {
                                if ('[' == term.Char)
                                {
                                    state = ParseTextState.BracketTerm;
                                }

                                continue;
                            }

                            break;
                        }

                        case ParseTextState.BracketTerm:
                        {
                            var token = ParseText(reader);

                            if (null == token)
                            {
                                break;
                            }

                            var term = token as TermToken;

                            if (null != term)
                            {
                                if ('[' == term.Char)
                                {
                                    buffer.Append(term.Char);
                                    state = ParseTextState.Text;

                                    continue;
                                }

                                if ('\\' == term.Char)
                                {
                                    state = ParseTextState.SlashTerm;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        void IDisposable.Dispose()
        {
            if (false == disposed)
            {
                Dispose(true);
            }
        }

        private void EnsureNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(String.Empty);
            }
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

                }
            }
            finally
            {
                disposed = true;
            }
        }

        private static IToken ParseText(StringReader reader)
        {
            var terms = new[]
            {
                '[', ']', '\\', '='
            };

            var text = new StringBuilder();

            while (true)
            {
                if (IsTerm(terms, reader.Current))
                {
                    if (0 == text.Length)
                    {
                        return new TermToken(reader.Current);
                    }

                    return new TextToken(text.ToString());
                }

                text.Append(reader.Current);

                if (false == reader.Advance())
                {
                    break;
                }
            }

            if (0 < text.Length)
            {
                return new TextToken(text.ToString());
            }

            return null;
        }

/*
        private static TagToken ReadTag(StringReader reader)
        {
            var state = OpenTagReadState.Unknown;
            var name = new StringBuilder();

            var value = reader.ReadNext();

            while (true)
            {
                switch (state)
                {
                    case OpenTagReadState.Unknown:
                    {
                        if (-1 == value)
                        {
                            return null;
                        }

                        state = OpenTagReadState.Opening;

                        break;
                    }

                    case OpenTagReadState.Opening:
                    {
                        var ch = (char) value;

                        if ('/' == ch)
                        {
                            state = OpenTagReadState.ClosingName;
                        }
                        else if ('[' == ch)
                        {
                            state = OpenTagReadState.SquareBracket;
                        }
                        else if (Char.IsLetter(ch))
                        {
                            name.Append(ch);
                            state = OpenTagReadState.Name;

                            continue;
                        }

                        break;
                    }

                    case OpenTagReadState.Name:
                    {
                        value = reader.ReadNext();

                        if (-1 == value)
                        {
                            return null;
                        }

                        var ch = (char) value;

                        if (Char.IsLetterOrDigit(ch) || '-' == ch)
                        {
                            name.Append(ch);
                        }
                        else if (']' == ch)
                        {
                            return new Tag(TagMode.Opening, name.ToString());
                        }

                        break;
                    }

                    case OpenTagReadState.ClosingName:
                    {
                        value = reader.ReadNext();

                        if (-1 == value)
                        {
                            return null;
                        }

                        var ch = (char) value;

                        if (Char.IsLetterOrDigit(ch) || '-' == ch)
                        {
                            name.Append(ch);
                        }
                        else if (']' == ch)
                        {
                            return new Tag(TagMode.Closing, name.ToString());
                        }

                        break;
                    }
                }
            }

            return null;
        }
*/

        private static bool IsTerm(char[] terms, char ch)
        {
            foreach (var term in terms)
            {
                if (term == ch)
                {
                    return true;
                }
            }

            return false;
        }

        private interface IToken
        {
        }

        private class TextToken : IToken
        {
            public string Text
            {
                get;
            }

            public TextToken(string text)
            {
                Text = text;
            }
        }

        private class TermToken : IToken
        {
            public char Char
            {
                get;
            }

            public TermToken(char ch)
            {
                Char = ch;
            }
        }

        private enum ParseTextState
        {
            Failed = -1,
            Unknown,
            Opening,
            Name,
            BracketTerm,
            ClosingName,
            Close,
            Text,
            SlashTerm
        }
    }
}