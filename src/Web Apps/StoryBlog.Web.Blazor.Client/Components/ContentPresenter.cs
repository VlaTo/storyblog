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
                (source, handler) => source.StateChanged += handler,
                (source, handler) => source.StateChanged -= handler,
                OnSourceStateChanged
            );
        }

        private void OnSourceStateChanged(object sender, TSource e) => ToggleLoadingSpinner();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(1, "div");
            BuildContent(2, builder);
            builder.CloseElement();
        }

        protected override void OnDispose()
        {
            eventManager.ClearWeakEventListeners();
            CancelExistingLoading();
        }

        private void ToggleLoadingSpinner()
        {
            CancelExistingLoading();

            if (Source.Value is IHasModelStatus model)
            {
                if (model.IsLoading())
                {
                    cts = new CancellationTokenSource();
                    ModalService.ShowAsync(new LoadingContent(), cts.Token).RunAndForget();
                }
            }
        }

        private void BuildContent(int sequence, RenderTreeBuilder builder)
        {
            if (Source.Value is IHasModelStatus model)
            {
                if (model.IsFailed())
                {
                    builder.AddContent(sequence, Failure);
                    return;
                }

                if (model.IsNone())
                {
                    builder.AddContent(sequence, NoContent);
                    return;
                }

                builder.AddContent(sequence, Content, Source.Value);

                return;
            }

            builder.AddContent(sequence, NoContent);
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