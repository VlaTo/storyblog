using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    internal class MarkDownTokenizer : IDisposable
    {
        private const string terminals = "~!#_*()[]";

        private MarkDownReader reader;
        private bool disposed;
        private bool hasCurrentLine;
        private string currentLine;
        private int currentPosition;

        public MarkDownTokenizer(TextReader reader)
        {
            this.reader = new MarkDownReader(reader);
        }

        public async Task<MarkDownToken> GetNextTokenAsync()
        {
            EnsureNotDisposed();

            var builder = new Lazy<StringBuilder>();

            if (null == currentLine)
            {
                var line = await reader.ReadLineAsync().ConfigureAwait(false);

                if (null == line)
                {
                    return null;
                }

                currentLine = line;
                currentPosition = 0;

                if (false == hasCurrentLine)
                {
                    hasCurrentLine = true;
                }
                else
                {
                    return new MarkDownTerminal('\n');
                }
            }

            while (currentPosition < currentLine.Length)
            {
                var ch = currentLine[currentPosition];

                if (IsTerminal(ch))
                {
                    if (builder.IsValueCreated)
                    {
                        var text = builder.Value.ToString();
                        return new MarkDownText(text);
                    }

                    currentPosition++;

                    return new MarkDownTerminal(ch);
                }

                builder.Value.Append(ch);
                currentPosition++;
            }

            currentLine = null;

            if (builder.IsValueCreated)
            {
                var text = builder.Value.ToString();
                return new MarkDownText(text);
            }

            return null;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void EnsureNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        private bool IsTerminal(char symbol) =>
            Char.IsControl(symbol) || Char.IsSeparator(symbol) || Char.IsWhiteSpace(symbol) || -1 < terminals.IndexOf(symbol);

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