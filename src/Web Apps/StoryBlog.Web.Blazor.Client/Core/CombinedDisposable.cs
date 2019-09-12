using System;

namespace StoryBlog.Web.Blazor.Client.Core
{
    internal class CombinedDisposable : IDisposable
    {
        private IDisposable[] disposables;
        private bool disposed;

        public CombinedDisposable(params IDisposable[] disposables)
        {
            if (null == disposables)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            this.disposables = disposables;
        }

        public void Dispose()
        {
            Dispose(true);
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
                    Array.ForEach(disposables, disposable => disposable.Dispose());
                    disposables = Array.Empty<IDisposable>();
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}