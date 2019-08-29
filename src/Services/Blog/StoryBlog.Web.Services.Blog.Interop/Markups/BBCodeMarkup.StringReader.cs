using System;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    public partial class BBCodeMarkup
    {
        /// <summary>
        /// 
        /// </summary>
        private sealed class StringReader : IDisposable
        {
            private const int NoPosition = -1;
            private const int EOS = -1;

            private string text;
            private int position;
            private int current;
            private bool disposed;

            /// <summary>
            /// 
            /// </summary>
            public int Current
            {
                get
                {
                    EnsureNotDisposed();

                    if (NoPosition == position)
                    {
                        throw new InvalidOperationException();
                    }

                    return current;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="text"></param>
            public StringReader(string text)
            {
                this.text = text;
                position = NoPosition;
                current = EOS;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>
            /// If there is 
            /// </returns>
            public bool Advance()
            {
                EnsureNotDisposed();

                if (NoPosition == position)
                {
                    if (String.IsNullOrEmpty(text))
                    {
                        return false;
                    }

                    position = 0;
                    current = text[position];

                    return true;
                }

                if (position >= text.Length)
                {
                    return false;
                }

                var advanced = ++position < text.Length;

                if (advanced)
                {
                    current = text[position];
                    return true;
                }

                current = EOS;

                return false;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
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
                    throw new ObjectDisposedException(nameof(StringReader));
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
                        text = null;
                        position = -1;
                        current = EOS;
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