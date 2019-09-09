using System;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using StoryBlog.Web.Blazor.Client.Core;
using StoryBlog.Web.Blazor.Components;

namespace StoryBlog.Web.Blazor.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapModalPresenter : BootstrapComponentBase
    {
        private static readonly ClassBuilder<BootstrapModalPresenter> classNameBuilder;
        private IModalContent modalContent;
        private string classString;
        private string titleLabelId;
        private bool isVisible;

        [Inject]
        public IModalService ModalService
        {
            get;
            set;
        }

        [Inject]
        public IIdManager IdManager
        {
            get; 
            set;
        }

        [Parameter]
        public bool IsOutline
        {
            get; 
            set;
        }

        [Parameter]
        public BootstrapButtonSizes ButtonSize
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
            modalContent = null;

            ButtonSize = BootstrapButtonSizes.Default;
            IsOutline = false;
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

        public void ShowModal(IModalContent content)
        {
            if (null == content)
            {
                throw new ArgumentNullException(nameof(content));
            }

            modalContent = content;

            IsVisible = true;

            Refresh();
            StateHasChanged();
        }

        public void CloseModal()
        {
            DoCloseModal(ModalButtons.CancelButton);
        }

        protected override void OnInitialized()
        {
            ModalService.OnShow += ShowModal;
            ModalService.OnClose += CloseModal;

            Refresh();
        }

        protected override void OnDispose()
        {
            ModalService.OnClose -= CloseModal;
            ModalService.OnShow -= ShowModal;
        }

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
            if (null != modalContent)
            {
                modalContent.SetResult(button);
                modalContent = null;
            }

            IsVisible = false;

            Refresh();
            StateHasChanged();
        }

        private void Refresh()
        {
            classString = classNameBuilder.Build(this, Class);
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

            if (null != modalContent && IsVisible)
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

            if (false == String.IsNullOrEmpty(modalContent.Title))
            {
                builder.OpenElement(sequence++, "h5");
                builder.AddAttribute(sequence++, "id", titleLabelId);
                builder.AddAttribute(sequence++, "class", "modal-title");
                builder.AddContent(sequence++, modalContent.Title);
                builder.CloseElement();
            }

            builder.OpenElement(sequence++, "button");
            builder.AddAttribute(sequence++, "class", "close");
            builder.AddAttribute(sequence++, "aria-label", "close");

            var callback = EventCallback.Factory.Create<ModalButton>(this, () => DoCloseModal(ModalButtons.CancelButton));
            builder.AddAttribute(sequence++, "onclick", callback);

            builder.OpenElement(sequence++, "span");
            builder.AddAttribute(sequence++, "aria-hidden", true);
            builder.AddContent(sequence++, "x");
            builder.CloseElement();

            builder.CloseElement();

            builder.CloseElement();
        }

        private void BuildModalContent(ref int sequence, RenderTreeBuilder builder)
        {
            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "modal-body");
            builder.AddContent(sequence++, modalContent.Content);
            builder.CloseElement();
        }

        private void BuildModalFooter(ref int sequence, RenderTreeBuilder builder)
        {
            if (ModalButtons.NoButtons == modalContent.Buttons)
            {
                return;
            }

            builder.OpenElement(sequence++, "div");
            builder.AddAttribute(sequence++, "class", "modal-footer");

            foreach (var button in modalContent.Buttons)
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
            var last = Array.IndexOf(modalContent.Buttons, button) == (modalContent.Buttons.Length - 1);
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
    }
}