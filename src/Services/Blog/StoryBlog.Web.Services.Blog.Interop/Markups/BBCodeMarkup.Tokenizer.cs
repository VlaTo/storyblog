using System;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    public partial class BBCodeMarkup
    {
        private sealed class Tokenizer : IDisposable
        {
            private const string Terms = "[]\\=";

            private StringReader reader;
            private char? lastChar;
            private bool disposed;

            public Tokenizer(StringReader reader)
            {
                this.reader = reader;
            }

            public void Dispose()
            {
                Dispose(true);
            }

            public IToken NextToken()
            {
                var text = new StringBuilder();

                while (Advance())
                {
                    if (-1 == GetCurrentChar())
                    {
                        break;
                    }

                    var ch = (char) GetCurrentChar();

                    if (-1 < Terms.IndexOf(ch))
                    {
                        if (0 == text.Length)
                        {
                            lastChar = null;
                            return new TermToken(ch);
                        }

                        lastChar = ch;

                        return new TextToken(text.ToString());
                    }

                    text.Append(ch);

                    lastChar = null;
                }

                if (0 < text.Length)
                {
                    return new TextToken(text.ToString());
                }

                return null;
            }

            private bool Advance()
            {
                if (lastChar.HasValue)
                {
                    return true;
                }

                return reader.Advance();
            }

            private int GetCurrentChar()
            {
                if (lastChar.HasValue)
                {
                    return lastChar.Value;
                }

                return reader.Current;
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
                        reader = null;
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}