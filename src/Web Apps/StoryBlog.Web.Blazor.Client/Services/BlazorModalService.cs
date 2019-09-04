using System;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal class BlazorModalService : IModalService
    {
        public event Action<string, RenderFragment> OnShow;

        public event Action OnClose;

        public void Show(string title, Type contentType)
        {
            if (contentType.BaseType != typeof(ComponentBase))
            {
                throw new ArgumentException("", nameof(contentType));
            }

            var content = new RenderFragment(x =>
            {
                x.OpenComponent(1, contentType);
                x.CloseComponent();
            });
            
            OnShow?.Invoke(title, content);
        }

        public void Close()
        {
            OnClose?.Invoke();
        }
    }
}