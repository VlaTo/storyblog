using System.Diagnostics;
using Blazor.Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using StoryBlog.Web.Client.Core;
using StoryBlog.Web.Client.Extensions;
using StoryBlog.Web.Client.Store;
using System.Threading;

namespace StoryBlog.Web.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class ContentPresenter<TSource> : BootstrapComponentBase
    {
        private CancellationTokenSource cts;
        private RenderFragment render;
        private readonly WeakEventManager eventManager;

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public IModalService ModalService
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public IState<TSource> Source
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<TSource> Content
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment NoContent
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment Failure
        {
            get; 
            set;
        }

        public ContentPresenter()
        {
            eventManager = new WeakEventManager();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            eventManager.AddWeakEventListener<IState<TSource>, TSource>(
                Source,
                (source, handler) =>
                {
                    Debug.WriteLine("ContentPresenter.OnInitialized: Subscribe to StateChanged");
                    source.StateChanged += handler;
                },
                (source, handler) =>
                {
                    Debug.WriteLine("ContentPresenter.OnInitialized: Subscribe to StateChanged");
                    source.StateChanged -= handler;
                },
                OnSourceStateChanged
            );

            ToggleLoadingSpinner();
        }

        private void OnSourceStateChanged(object sender, TSource e)
        {
            Debug.WriteLine("ContentPresenter.OnSourceStateChanged");
            ToggleLoadingSpinner();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, "div");
            builder.AddContent(1, render);
            builder.CloseElement();
        }

        protected override void OnDispose()
        {
            eventManager.ClearWeakEventListeners();
            CancelExistingLoading();
        }

        private void ToggleLoadingSpinner()
        {
            if (Source.Value is IHasModelStatus model)
            {
                Debug.WriteLine($"ContentPresenter.ToggleLoadingSpinner status: {model.Status.State}");

                CancelExistingLoading();

                if (model.IsFailed())
                {
                    render = Failure;
                    return;
                }

                if (model.IsNone())
                {
                    render = NoContent;
                    return;
                }

                if (model.IsLoading())
                {
                    cts = new CancellationTokenSource();
                    ModalService.ShowAsync(new LoadingContent(), cts.Token).RunAndForget();
                }

                render = Content.Invoke(Source.Value);
            }
            else
            {
                Debug.WriteLine("ContentPresenter.ToggleLoadingSpinner no status");
            }
        }

        private void CancelExistingLoading()
        {
            if (null != cts)
            {
                cts?.Cancel();
                cts.Dispose();
            }

            cts = null;
        }
    }
}