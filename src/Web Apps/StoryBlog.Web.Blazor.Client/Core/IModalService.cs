using System;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Components;

namespace StoryBlog.Web.Blazor.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModalService
    {
        /// <summary>
        /// 
        /// </summary>
        event Action<IModalContent> OnShow;

        /// <summary>
        /// 
        /// </summary>
        event Action OnClose;

        /// <summary>
        /// 
        /// </summary>
        Task<ModalButton> ShowAsync(string title, Type contentType, params ModalButton[] buttons);

        /// <summary>
        /// 
        /// </summary>
        void Close();
    }
}