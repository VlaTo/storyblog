using StoryBlog.Web.Client.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Core
{
    public interface IModalContentObserver
    {
        void ShowContent(IModalContent content, TaskCompletionSource<ModalButton> completion, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IModalService
    {
        /// <summary>
        /// 
        /// </summary>
        Task<ModalButton> ShowAsync(IModalContent modalContent, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        IDisposable Subscribe<T>(IModalContentObserver observer) where T : IModalContent;
    }
}