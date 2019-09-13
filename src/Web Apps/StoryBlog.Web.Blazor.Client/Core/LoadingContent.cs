using StoryBlog.Web.Client.Components;

namespace StoryBlog.Web.Client.Core
{
    public sealed class LoadingContent : IModalContent
    {
        public string Title => null;

        public ModalButton[] Buttons => ModalButtons.NoButtons;
    }
}