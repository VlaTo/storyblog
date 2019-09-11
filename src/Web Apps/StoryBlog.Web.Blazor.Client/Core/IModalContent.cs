using StoryBlog.Web.Blazor.Client.Components;

namespace StoryBlog.Web.Blazor.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModalContent
    {
        /// <summary>
        /// 
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        ModalButton[] Buttons
        {
            get;
        }
    }
}