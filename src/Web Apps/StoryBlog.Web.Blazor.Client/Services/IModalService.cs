using System;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Client.Services
{
    public interface IModalService
    {
        event Action<string, RenderFragment> OnShow;

        event Action OnClose;

        void Show(string title, Type contentType);

        void Close();
    }
}