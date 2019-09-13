using StoryBlog.Web.Client.Components;

namespace StoryBlog.Web.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class InformationContent : IModalContent
    {
        public string Title
        {
            get;
        }

        public string Description
        {
            get;
        }

        public ModalButton[] Buttons
        {
            get;
        }

        public InformationContent(string title, string description, params ModalButton[] buttons)
        {
            Title = title;
            Description = description;
            Buttons = buttons;
        }
    }
}