using System;
using StoryBlog.Web.Blazor.Client.Components;

namespace StoryBlog.Web.Blazor.Client.Core
{
    public sealed class LoadingContent : IModalContent
    {
        public string Title => null;

        public ModalButton[] Buttons => ModalButtons.NoButtons;
    }
}