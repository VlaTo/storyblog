using System;

namespace LibraProgramming.Windows.UI.Xaml.Core.Markups
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class StringReader : IDisposable
    {
        private const int NoPosition = -1;

        private string text;
        private int position;
        private bool disposed;

        /// <summary>
        /// 
        /// </summary>
        public char Current
        {
            get
            {
                EnsureNotDisposed();

                if (NoPosition == position)
                {
                    throw new InvalidOperationException();
                }

                return text[position];
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

                return true;
            }

            if (position >= text.Length)
            {
                return false;
            }

            ++position;

            return true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
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
                throw new ObjectDisposedException("StringReader");
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
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}