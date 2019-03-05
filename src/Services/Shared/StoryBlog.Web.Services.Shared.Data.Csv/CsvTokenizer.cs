using StoryBlog.Web.Services.Shared.Data.Csv.Tokens;
using System;
using System.IO;
using System.Text;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    internal class CsvTokenizer : IDisposable
    {
        private const int EndOfStream = -1;

        private static readonly char[] terminals;

        private readonly TextReader reader;
        private bool disposed;
        private TokenizerState state;
        private int input;

        public TextPosition TextPosition
        {
            get;
            private set;
        }

        public CsvTokenizer(TextReader reader)
        {
            this.reader = reader;
            TextPosition = TextPosition.Empty;
        }

        static CsvTokenizer()
        {
            terminals = new[]
            {
                CsvTerminals.Comma,
                CsvTerminals.DoubleQuote,
                CsvTerminals.Whitespace,
                CsvTerminals.LineFeed,
                CsvTerminals.NewLine,
                CsvTerminals.Tab,
                '!',
                '@',
                '#',
                '$',
                '%',
                '&',
                '*',
                '(',
                ')',
                '-',
                '_',
                '=',
                '+',
                '[',
                ']',
                '{',
                '}',
                ':',
                ';',
                '\\',
                '/'
            };
        }

        public CsvToken GetToken()
        {
            if (disposed)
            {
                throw new ObjectDisposedException("tokenizer");
            }

            StringBuilder text = null;

            while (true)
            {
                switch (state)
                {
                    case TokenizerState.Unknown:
                    {
                        input = reader.Read();

                        if (EndOfStream != input)
                        {
                            TextPosition = TextPosition.Begin();
                            //text = new StringBuilder();
                            state = TokenizerState.Reading;

                            break;
                        }

                        state = TokenizerState.EndOfDocument;

                        break;
                    }

                    case TokenizerState.FlushLastToken:
                    {
                        if (null != text && 0 < text.Length)
                        {
                            state = TokenizerState.EndOfDocument;
                            return CsvToken.String(text.ToString());
                        }

                        state = TokenizerState.Failed;

                        break;
                    }

                    case TokenizerState.EndOfDocument:
                    {
                        return CsvToken.End;
                    }

                    case TokenizerState.Reading:
                    {
                        if (EndOfStream == input)
                        {
                            state = null == text ? TokenizerState.EndOfDocument : TokenizerState.FlushLastToken;
                            break;
                        }

                        if (null == text)
                        {
                            //TextPosition = TextPosition.Begin();
                            text = new StringBuilder();
                        }

                        var current = (char) input;

                        if (IsTerm(current))
                        {
                            if (0 < text.Length)
                            {
                                return CsvToken.String(text.ToString());
                            }

                            input = reader.Read();

                            return CsvToken.Terminal(current);
                        }

                        text.Append(current);

                        input = reader.Read();

                        break;
                    }

                    default:
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private static bool IsTerm(char ch)
        {
            return 0 <= Array.IndexOf(terminals, ch);
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
                    reader.Dispose();
                }
            }
            finally
            {
                disposed = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private enum TokenizerState
        {
            Failed = -1,
            Unknown,
            Reading,
            FlushLastToken,
            EndOfDocument
        }
    }
}