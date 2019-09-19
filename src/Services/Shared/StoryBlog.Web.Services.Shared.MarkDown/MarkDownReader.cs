using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    internal class MarkDownReader : IDisposable
    {
        private TextReader reader;
        private char[] pendingBuffer;
        private bool disposed;
        private int pendingPosition;
        private int pendingCount;
        private bool eos;

        public MarkDownReader(TextReader reader)
        {
            this.reader = reader;
            pendingBuffer = new char[128];
            pendingPosition = 0;
            pendingCount = 0;
        }

        public async Task<string> ReadLineAsync()
        {
            EnsureNotDisposed();

            if (eos)
            {
                return null;
            }

            try
            {
                var builder = new StringBuilder();
                var completed = false;

                while (false == completed)
                {
                    if (false == eos && 0 == pendingCount)
                    {
                        var requiredCount = pendingBuffer.Length - (pendingPosition + pendingCount);
                        var offset = pendingPosition + pendingCount;
                        var count = await reader
                            .ReadAsync(pendingBuffer, offset, requiredCount)
                            .ConfigureAwait(false);

                        if (0 == count)
                        {
                            eos = true;
                        }
                        else
                        {
                            pendingCount += count;
                        }
                    }

                    if (0 == pendingCount)
                    {
                        completed = true;
                        continue;
                    }

                    var eol = false;

                    for (; false == eol && 0 < pendingCount; pendingPosition++, pendingCount--)
                    {
                        if ('\n' == pendingBuffer[pendingPosition])
                        {
                            eol = true;
                            completed = true;

                            continue;
                        }

                        if ('\r' == pendingBuffer[pendingPosition])
                        {
                            continue;
                        }

                        builder.Append(pendingBuffer[pendingPosition]);
                    }

                    if (0 < pendingCount)
                    {
                        Array.Copy(pendingBuffer, pendingPosition, pendingBuffer, 0, pendingCount);
                    }

                    pendingPosition = 0;
                }

                return 0 < builder.Length ? builder.ToString() : null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
                    pendingBuffer = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}