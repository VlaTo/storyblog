using System;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class BBCodeMarkup : IDisposable
    {
        private bool disposed;


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
    }
}