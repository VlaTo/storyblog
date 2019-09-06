using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Components
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
        Task ShowAsync(string title, Type contentType);

        /// <summary>
        /// 
        /// </summary>
        void Close();
    }
}