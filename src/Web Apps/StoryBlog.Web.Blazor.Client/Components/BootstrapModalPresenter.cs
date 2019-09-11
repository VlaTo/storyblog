using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using StoryBlog.Web.Blazor.Client.Core;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Client.Extensions;

namespace StoryBlog.Web.Blazor.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapModalPresenter : BootstrapComponentBase, IModalContentObserver
    {
        private static readonly ClassBuilder<BootstrapModalPresenter> ClassNameBuilder;
        private IDisposable subscription1;
        private IDisposable subscription2;
        private ModalContext modalContext;
        private string classString;
        private string titleLabelId;
        private bool isVisible;

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
        [Inject]
        public IIdManager IdManager
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Inject]
        public IJSRuntime JsRuntime
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public bool IsOutline
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public BootstrapButtonSizes ButtonSize
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<InformationContent> Information
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public RenderFragment<LoadingContent> Loading
        {
            get; 
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible
        {
            get => isVisible;
            private set
            {
                if (isVisible == value)
                {
                    return;
                }

                isVisible = value;

                Refresh();
                StateHasChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public BootstrapModalPresenter()
        {
            modalContext = null;

            ButtonSize = BootstrapButtonSizes.Default;
            IsOutline = false;
        }

        static BootstrapModalPresenter()
        {
            ClassNameBuilder = new ClassBuilder<BootstrapModalPresenter>(null)
                .DefineClass(@class => @class.Name("modal"))
                .DefineClass(@class => @class.Name("fade"))
                .DefineClass(
                    @class => @class.Name("show").Condition(component => component.IsVisible)
                );
        }

        /// <inheritdoc cref="IModalContentObserver.ShowContent" />
        void IModalContentObserver.ShowContent(IModalContent content, TaskCompletionSource<ModalButton> completion, CancellationToken cancellationToken)
        {
            if (null == content)
            {
                throw new ArgumentNullException(nameof(content));
            }

            modalContext = new ModalContext(this, content, completion, cancellationToken);

            UpdateVisibility(true);
        }

        /// <inheritdoc cref="ComponentBase.OnInitialized" />
        protected override void OnInitialized()
        {
            subscription1 = ModalService.Subscribe<InformationContent>(this);
            subscription2 = ModalService.Subscribe<LoadingContent>(this);

            Refresh();
        }

        /// <inheritdoc cref="BootstrapComponentBase.OnDispose" />
        protected override void OnDispose()
        {
            subscription1.Dispose();
            subscription2.Dispose();
        }

        /// <inheritdoc cref="ComponentBase.BuildRenderTree" />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            EnsureTitleLabelId();

            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", classString);
            builder.AddAttribute(2, "style", $"display: " + (IsVisible ? "block" : "none") + ";");
            builder.AddAttribute(3, "tabindex", -1);
            builder.AddAttribute(4, "role", "dialog");
            builder.AddAttribute(5, "aria-modal", true);
            builder.AddAttribute(6, "aria-labelledby", titleLabelId);

            BuildDialog(7, builder);
            
            builder.CloseElement();
        }

        private void DoCloseModal(ModalButton button)
        {
            Debug.WriteLine("BootstrapModalPresenter.DoCloseModal executing");

            if (null != modalContext)
            {
                Debug.WriteLine("BootstrapModalPresenter.DoCloseModal try to complete modal context");
                modalContext.SetResult(button);
                modalContext = null;
                Debug.WriteLine("BootstrapModalPresenter.DoCloseModal completing modal context");
            }

            UpdateVisibility(false);

            Debug.WriteLine("BootstrapModalPresenter.DoCloseModal executed");
        }

        private void UpdateVisibility(bool visible)
        {
            var toggle = IsVisible != visible;
            
            IsVisible = visible;

            if (toggle)
            {
                JsRuntime
                    .InvokeVoidAsync("toggleBody", CancellationToken.None, visible)
                    .RunAndForget();
            }
        }

        private void Refresh()
        {
            classString = ClassNameBuilder.Build(this, Class);
        }

        private void EnsureTitleLabelId()
        {
            if (null == titleLabelId)
            {
                titleLabelId = IdManager.GenerateId("modalPrefixId");
            }
        }

        private void BuildDialog(int sequence, RenderTreeBuilder builder)
        {
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "modal-dialog modal-ld modal-dialog-centered");
            builder.AddAttribute(sequence++, "role", "document");

            if (null != modalContext && IsVisible)
            {
                BuildModalContent(sequence, builder);
            }

            builder.CloseElement();
        }

        private void BuildModalContent(int sequence, RenderTreeBuilder builder)
        {
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "modal-content");

            BuildModalHeader(ref sequence, builder);

            BuildModalContent(ref sequence, builder);

            BuildModalFooter(ref sequence, builder);

            builder.CloseElement();
        }

        private void BuildModalHeader(ref int sequence, RenderTreeBuilder builder)
        {
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "modal-header");

            if (false == String.IsNullOrEmpty(modalContext.Title))
            {
                builder.OpenElement(sequence++, "h5");
                builder.AddAttribute(sequence++, "id", titleLabelId);
                builder.AddAttribute(sequence++, "class", "modal-title");
                builder.AddContent(sequence++, modalContext.Title);
                builder.CloseElement();
            }

            {
                builder.OpenElement(sequence++, "button");
                builder.AddAttribute(sequence++, "type", "button");
                builder.AddAttribute(sequence++, "class", "close");
                builder.AddAttribute(sequence++, "aria-label", "Close");

                var callback =
                    EventCallback.Factory.Create<ModalButton>(this, () => DoCloseModal(ModalButtons.CancelButton));
                builder.AddAttribute(sequence++, "onclick", callback);

                {
                    builder.OpenElement(sequence++, "span");
                    builder.AddAttribute(sequence++, "aria-hidden", true);
                    builder.AddContent(sequence++, new MarkupString("&times;"));
                    builder.CloseElement();
                }

                builder.CloseElement();
            }

            builder.CloseElement();
        }

        private void BuildModalContent(ref int sequence, RenderTreeBuilder builder)
        {
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "modal-body");

            if (modalContext.Content is InformationContent info)
            {
                builder.AddContent(sequence++, Information, info);
            }
            else if (modalContext.Content is LoadingContent loading)
            {
                builder.AddContent(sequence++, Loading, loading);
            }
            else if (modalContext.Content is ModalContent modal)
            {
                builder.AddContent(sequence++, modal.Content);
            }

            builder.CloseElement();
        }

        private void BuildModalFooter(ref int sequence, RenderTreeBuilder builder)
        {
            if (ModalButtons.NoButtons == modalContext.Buttons || 0 == modalContext.Buttons.Length)
            {
                return;
            }

            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "modal-footer");

            foreach (var button in modalContext.Buttons)
            {
                var callback = EventCallback.Factory.Create<ModalButton>(this, () => DoCloseModal(button));

                builder.OpenElement(sequence++, "button");
                builder.AddAttribute(sequence++, "type", "button");
                builder.AddAttribute(sequence++, "class", GetButtonClass(button));
                builder.AddAttribute(sequence++, "onclick", callback);

                builder.AddContent(sequence++, button.Title);

                builder.CloseElement();
            }

            builder.CloseElement();
        }

        private string GetButtonClass(ModalButton button)
        {
            var last = Array.IndexOf(modalContext.Buttons, button) == (modalContext.Buttons.Length - 1);
            var builder = new StringBuilder("btn ");

            if (IsOutline)
            {
                builder.Append(last ? "btn-outline-primary" : "btn-outline-secondary");
            }
            else
            {
                builder.Append(last ? "btn-primary" : "btn-secondary");
            }

            builder.Append(' ');

            switch (ButtonSize)
            {
                case BootstrapButtonSizes.Default:
                {
                    break;
                }

                case BootstrapButtonSizes.Large:
                {
                    builder.Append("btn-lg");
                    break;
                }

                case BootstrapButtonSizes.Small:
                {
                    builder.Append("btn-sm");
                    break;
                }
            }
            
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class ModalContext
        {
            private readonly BootstrapModalPresenter presenter;
            private readonly TaskCompletionSource<ModalButton> completion;
            private readonly CancellationToken cancellationToken;
            private CancellationTokenRegistration registration;

            public string Title => Content.Title;

            public IModalContent Content
            {
                get;
            }

            public ModalButton[] Buttons => Content.Buttons;

            public ModalContext(BootstrapModalPresenter presenter, IModalContent content, TaskCompletionSource<ModalButton> completion, CancellationToken cancellationToken)
            {
                Content = content;

                this.presenter = presenter;
                this.completion = completion;
                this.cancellationToken = cancellationToken;

                if (default != cancellationToken)
                {
                    Debug.WriteLine("ModalContext subscribe to cancellationToken");
                    registration = cancellationToken.Register(Cancel);
                }
            }

            public void SetResult(ModalButton button)
            {
                Debug.WriteLine("ModalContext.SetResult try to complete modal context");
                if (default != cancellationToken)
                {
                    Debug.WriteLine("ModalContext.SetResult registration.Disposing");
                    registration.Dispose();
                    Debug.WriteLine("ModalContext.SetResult registration.Disposed");
                }

                Debug.WriteLine("ModalContext.SetResult completion.SetResult start");
                completion.SetResult(button);
                Debug.WriteLine("ModalContext.SetResult completion.SetResult complete");
            }

            private void Cancel()
            {
                Debug.WriteLine("CancellationToken.Cancel was detected");
                presenter.DoCloseModal(null);
            }
        }
    }
}