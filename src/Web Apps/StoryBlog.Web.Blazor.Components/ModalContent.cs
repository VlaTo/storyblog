using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ModalContent : IModalContent
    {
        public string Title
        {
            get;
        }

        public RenderFragment Content
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

            Title = title;
            Content = content;
        }

        public ModalContent(string title, Type contentType)
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

            Title = title;
            Content = target =>
            {
                target.OpenComponent(1, contentType);
                target.CloseComponent();
            };
        }

        public void OnCallback()
        {
            Debug.WriteLine("ModelContent.OnCallback");
        }
    }
}