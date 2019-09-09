using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModalContent : IModalContent
    {
        private TaskCompletionSource<ModalButton> tcs;

        public string Title
        {
            get;
        }

        public RenderFragment Content
        {
            get;
        }

        public ModalButton[] Buttons
        {
            get;
        }

        public ModalContent(string title, RenderFragment content)
        {
            if (null == title)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (null == content)
            {
                throw new ArgumentNullException(nameof(content));
            }

            tcs = new TaskCompletionSource<ModalButton>();

            Title = title;
            Content = content;
        }

        public ModalContent(string title, Type contentType, params ModalButton[] buttons)
        {
            if (null == title)
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (null == contentType)
            {
                throw new ArgumentNullException(nameof(contentType));
            }

            if (contentType.IsAssignableFrom(typeof(ComponentBase)))
            {
                throw new ArgumentException("", nameof(contentType));
            }

            tcs = new TaskCompletionSource<ModalButton>();

            Title = title;
            Buttons = buttons;
            Content = target =>
            {
                target.OpenComponent(1, contentType);
                target.CloseComponent();
            };
        }

        public Task<ModalButton> WaitForComplete() => tcs.Task;

        public void SetResult(ModalButton button) => tcs.SetResult(button);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ModalButton
    {
        public string Title
        {
            get;
        }

        public ModalButton(string title)
        {
            Title = title;
        }
    }
}