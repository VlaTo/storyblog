﻿using System;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Client.Components;

namespace StoryBlog.Web.Blazor.Client.Core
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