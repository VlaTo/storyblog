using System;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Client.Core;
using StoryBlog.Web.Blazor.Components;

namespace StoryBlog.Web.Blazor.Client.Components
{
    /// <inheritdoc cref="IModalService" />
    public sealed class BootstrapModalService : IModalService
    {
        /// <inheritdoc cref="OnShow" />
        public event Action<IModalContent> OnShow;

        /// <inheritdoc cref="OnClose" />
        public event Action OnClose;
        
        public Task<ModalButton> ShowAsync(string title, Type contentType, params ModalButton[] buttons)
        {
            var content = new ModalContent(title, contentType, buttons);
            OnShow?.Invoke(content);
            return content.WaitForComplete();
        }

        public void Close()
        {
            OnClose?.Invoke();
        }
    }
}