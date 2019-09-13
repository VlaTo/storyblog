using StoryBlog.Web.Client.Components;

namespace StoryBlog.Web.Client.Core
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