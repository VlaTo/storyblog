using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Components
{
    /// <inheritdoc cref="IModalService" />
    public sealed class BootstrapModalService : IModalService
    {
        /// <inheritdoc cref="OnShow" />
        public event Action<IModalContent> OnShow;

        /// <inheritdoc cref="OnClose" />
        public event Action OnClose;
        
        public Task ShowAsync(string title, Type contentType)
        {
            modalContext = new ModalContext(modalContent.Title, modalContent.Content, () =>
            {
                modalContent.OnCallback();
                CloseModal();
            });

            OnShow?.Invoke(content);
        }

        public void Close()
        {
            OnClose?.Invoke();
        }
    }
}