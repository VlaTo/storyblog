using Microsoft.AspNetCore.Components;
using System;

namespace StoryBlog.Web.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapComponentBase : ComponentBase, IDisposable
    {
        private bool disposed;

        [Parameter]
        public string Class
        {
            get;
            set;
        }

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void OnDispose()
        {
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
                    OnDispose();
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}