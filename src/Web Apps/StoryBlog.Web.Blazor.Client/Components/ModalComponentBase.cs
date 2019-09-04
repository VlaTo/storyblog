using System;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Blazor.Client.Services;

namespace StoryBlog.Web.Blazor.Client.Components
{
    public class ModalComponentBase : ComponentBase, IDisposable
    {
        protected string Title
        {
            get; 
            set;
        }

        protected bool IsVisible
        {
            get; 
            set;
        }

        protected RenderFragment Content
        {
            get; 
            set;
        }

        [Inject]
        public IModalService ModalService
        {
            get; 
            set;
        }

        public void Dispose()
        {
            ModalService.OnShow -= DoShowModal;
            ModalService.OnClose -= DoCloseModal;
        }

        public void ShowModal(string title, RenderFragment content)
        {
            Title = title;
            Content = content;
            IsVisible = true;

            StateHasChanged();
        }

        public void CloseModal()
        {
            IsVisible = false;
            Title = String.Empty;
            Content = null;

            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            ModalService.OnShow += DoShowModal;
            ModalService.OnClose += DoCloseModal;
        }

        private void DoShowModal(string title, RenderFragment content)
        {
            ShowModal(title, content);
        }

        private void DoCloseModal()
        {
            CloseModal();
        }
    }
}