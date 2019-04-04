using System;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapComponentBase : ComponentBase, IDisposable
    {
        private bool disposed;

        [Parameter]
        protected string Class
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