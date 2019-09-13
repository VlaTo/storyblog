using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using StoryBlog.Web.Client.Core;
using StoryBlog.Web.Client.Extensions;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapModalPresenter : BootstrapComponentBase, IModalContentObserver
    {
        private static readonly ClassBuilder<BootstrapModalPresenter> ClassNameBuilder;
        private IDisposable subscription;
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
            subscription = new CombinedDisposable(
                ModalService.Subscribe<InformationContent>(this),
                ModalService.Subscribe<LoadingContent>(this)
            );

            Refresh();
        }

        /// <inheritdoc cref="BootstrapComponentBase.OnDispose" />
        protected override void OnDispose()
        {
            subscription.Dispose();
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

            BuildDialog(builder);
            
            builder.CloseElement();
        }

        private void BuildDialog(RenderTreeBuilder builder)
        {
            builder.OpenElement(7, "div");
            builder.AddAttribute(8, "class", "modal-dialog modal-ld modal-dialog-centered");
            builder.AddAttribute(9, "role", "document");

            if (null != modalContext && IsVisible)
            {
                BuildModal(builder);
            }

            builder.CloseElement();
        }

        private void BuildModal(RenderTreeBuilder builder)
        {
            builder.OpenElement(10, "div");
            builder.AddAttribute(11, "class", "modal-content");

            if (false == String.IsNullOrEmpty(modalContext.Title))
            {
                BuildModalHeader(builder);
            }

            BuildModalContent(builder);

            if (0 < modalContext.Buttons.Length)
            {
                BuildModalFooter(builder);
            }

            builder.CloseElement();
        }

        private void BuildModalHeader(RenderTreeBuilder builder)
        {
            builder.OpenElement(12, "div");
            builder.AddAttribute(13, "class", "modal-header");
            {
                builder.OpenElement(14, "h5");
                builder.AddAttribute(15, "id", titleLabelId);
                builder.AddAttribute(16, "class", "modal-title");
                builder.AddContent(17, modalContext.Title);
                builder.CloseElement();
                {
                    builder.OpenElement(18, "button");
                    builder.AddAttribute(19, "type", "button");
                    builder.AddAttribute(20, "class", "close");
                    builder.AddAttribute(21, "aria-label", "Close");

                    var callback = EventCallback.Factory.Create<ModalButton>(
                        this, () => DoCloseModal(ModalButtons.CancelButton)
                    );

                    builder.AddAttribute(22, "onclick", callback);
                    {
                        builder.OpenElement(23, "span");
                        builder.AddAttribute(24, "aria-hidden", true);
                        builder.AddContent(25, new MarkupString("&times;"));
                        builder.CloseElement();
                    }
                    builder.CloseElement();
                }
            }
            builder.CloseElement();
        }

        private void BuildModalContent(RenderTreeBuilder builder)
        {
            builder.OpenElement(26, "div");
            builder.AddAttribute(27, "class", "modal-body");

            if (modalContext.Content is InformationContent info)
            {
                builder.AddContent(28, Information, info);
            }
            else if (modalContext.Content is LoadingContent loading)
            {
                builder.AddContent(29, Loading, loading);
            }
            else if (modalContext.Content is ModalContent modal)
            {
                builder.AddContent(30, modal.Content);
            }

            builder.CloseElement();
        }

        private void BuildModalFooter(RenderTreeBuilder builder)
        {

            builder.OpenElement(31, "div");
            builder.AddAttribute(32, "class", "modal-footer");

            foreach (var button in modalContext.Buttons)
            {
                var callback = EventCallback.Factory.Create<ModalButton>(this, () => DoCloseModal(button));

                builder.OpenElement(33, "button");
                builder.AddAttribute(34, "type", "button");
                builder.AddAttribute(35, "class", GetButtonClass(button));
                builder.AddAttribute(36, "onclick", callback);

                builder.AddContent(37, button.Title);

                builder.CloseElement();
            }

            builder.CloseElement();
        }

        private void DoCloseModal(ModalButton button)
        {
            if (null != modalContext)
            {
                modalContext.SetResult(button);
                modalContext = null;
            }

            UpdateVisibility(false);
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
                    registration = cancellationToken.Register(Cancel);
                }
            }

            public void SetResult(ModalButton button)
            {
                if (default != cancellationToken)
                {
                    registration.Dispose();
                }

                completion.SetResult(button);
            }

            private void Cancel() => presenter.DoCloseModal(null);
        }
    }
}