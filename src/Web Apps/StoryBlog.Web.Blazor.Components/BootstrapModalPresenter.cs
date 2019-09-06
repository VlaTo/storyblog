using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace StoryBlog.Web.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class ModalContext
    {
        private readonly IModalContent modalContent;
        private readonly EventCallback onClose;

        public static ModalContext Empty
        {
            get;
        }

        public virtual string Title
        {
            get => modalContent.Title;
        }

        public virtual RenderFragment Content
        {
            get => modalContent.Content;
        }

        public virtual EventCallback OnClose
        {
            get => onClose;
        }

        public ModalContext(IModalContent modalContent, Action<IModalContent> callback)
        {
            if (null == modalContent)
            {
                throw new ArgumentNullException(nameof(modalContent));
            }

            this.modalContent = modalContent;
            EventCallback.Factory.Create(this,callback)
            onClose = EventCallback.Factory.Create(this, modalContent.OnCallback);
        }

        static ModalContext()
        {
            Empty = new EmptyModalContext();
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class EmptyModalContext : ModalContext
        {
            public override string Title => String.Empty;

            public override RenderFragment Content => _ => { };

            public override EventCallback OnClose => EventCallback.Empty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BootstrapModalPresenter : BootstrapComponentBase
    {
        private static readonly ClassBuilder<BootstrapModalPresenter> classNameBuilder;
        private ModalContext modalContext;
        private string classString;
        private bool isVisible;

        [Inject]
        public IModalService ModalService
        {
            get;
            set;
        }

        [Parameter]
        public RenderFragment<ModalContext> Content
        {
            get; 
            set;
        }

        [Parameter]
        public string LabelName
        {
            get; 
            set;
        }

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

        public BootstrapModalPresenter()
        {
            modalContext = ModalContext.Empty;
        }

        static BootstrapModalPresenter()
        {
            classNameBuilder = new ClassBuilder<BootstrapModalPresenter>(null)
                .DefineClass(@class => @class.Name("modal"))
                .DefineClass(@class => @class.Name("fade"))
                .DefineClass(
                    @class => @class.Name("show").Condition(component => component.IsVisible)
                );
        }

        public void ShowModal(IModalContent modalContent)
        {
            if (null == modalContent)
            {
                throw new ArgumentNullException(nameof(modalContent));
            }

            modalContext = new ModalContext(modalContent);

            IsVisible = true;

            Refresh();
            StateHasChanged();
        }

        public void CloseModal()
        {
            modalContext = null;

            Refresh();
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            ModalService.OnShow += ShowModal;
            ModalService.OnClose += DoCloseModal;

            Refresh();
        }

        protected override void OnDispose()
        {
            ModalService.OnClose -= DoCloseModal;
            ModalService.OnShow -= ShowModal;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "class", classString);
            builder.AddAttribute(1, "style", $"display: " + (IsVisible ? "block" : "none") + ";");
            builder.AddAttribute(2, "tabindex", -1);
            builder.AddAttribute(3, "role", "dialog");
            builder.AddAttribute(4, "aria-modal", true);
            builder.AddAttribute(5, "aria-labelledby", LabelName);

            builder.AddContent(6, Content, modalContext ?? ModalContext.Empty);
            
            builder.CloseElement();
        }

        private void Refresh()
        {
            classString = classNameBuilder.Build(this, Class);
        }

        private void DoCloseModal()
        {
            CloseModal();
        }
    }
}