using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using StoryBlog.Web.Blazor.Client.Components.Attributes;
using StoryBlog.Web.Blazor.Client.Core;
using StoryBlog.Web.Blazor.Components;

namespace StoryBlog.Web.Blazor.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public enum BootstrapButtonTypes
    {
        Default,

        /// <summary>
        /// 
        /// </summary>
        [Style("primary")]
        Primary,

        /// <summary>
        /// 
        /// </summary>
        [Style("secondary")]
        Secondary,

        /// <summary>
        /// 
        /// </summary>
        [Style("success")]
        Success,

        /// <summary>
        /// 
        /// </summary>
        [Style("danger")]
        Danger,

        /// <summary>
        /// 
        /// </summary>
        [Style("warning")]
        Warning,

        /// <summary>
        /// 
        /// </summary>
        [Style("info")]
        Info,

        /// <summary>
        /// 
        /// </summary>
        [Style("light")]
        Light,

        /// <summary>
        /// 
        /// </summary>
        [Style("dark")]
        Dark,

        /// <summary>
        /// 
        /// </summary>
        [Style("link")]
        Link
    }

    /// <summary>
    /// 
    /// </summary>
    public enum BootstrapButtonSizes
    {
        Default,
        Large,
        Small
    }

    /// <summary>
    /// 
    /// </summary>
    public class BootstrapButton : BootstrapComponentBase
    {
        private static readonly ClassBuilder<BootstrapButton> classNameBuilder;

        protected ElementReference button;

        [Parameter]
        public BootstrapButtonTypes Type
        {
            get;
            set;
        }

        [Parameter]
        public BootstrapButtonSizes Size
        {
            get;
            set;
        }

        [Parameter]
        public bool Active
        {
            get;
            set;
        }

        [Parameter]
        public bool Outline
        {
            get;
            set;
        }

        [Parameter]
        public bool Block
        {
            get;
            set;
        }

        [Parameter]
        public bool Disabled
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback<EventArgs> OnClick
        {
            get;
            set;
        }

        [Parameter]
        public RenderFragment ChildContent
        {
            get;
            set;
        }

        [Parameter]
        public string Style
        {
            get;
            set;
        }

        [Parameter]
        public int TabIndex
        {
            get;
            set;
        }

        protected string ClassString
        {
            get;
            private set;
        }

        public BootstrapButton()
        {
            Type = BootstrapButtonTypes.Default;
            Size = BootstrapButtonSizes.Default;
            Outline = false;
            Active = false;
            Block = false;
            Disabled = false;
            TabIndex = 0;
        }

        static BootstrapButton()
        {
            classNameBuilder = new ClassBuilder<BootstrapButton>("btn")
                .DefineClass(@class => @class
                    .Modifier("outline", component => component.Outline)
                    .Name(component => EnumHelper.GetClassName(component.Type))
                    .Condition(component => BootstrapButtonTypes.Default != component.Type)
                )
                .DefineClass(@class => @class.Name("block").Condition(component => component.Block))
                .DefineClass(@class => @class.Name("lg").Condition(component => BootstrapButtonSizes.Large == component.Size))
                .DefineClass(@class => @class.Name("sm").Condition(component => BootstrapButtonSizes.Small == component.Size))
                .DefineClass(@class => @class.NoPrefix().Name("active").Condition(component => component.Active));
        }

        protected override void OnInitialized()
        {
            ClassString = classNameBuilder.Build(this, Class);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "button");

            builder.AddAttribute(1, "type", "button");
            builder.AddAttribute(2, "class", ClassString);
            builder.AddAttribute(3, "style", Style);
            builder.AddAttribute(4, "tabindex", TabIndex);
            builder.AddAttribute(5, "disabled", Disabled);
            builder.AddAttribute(6, "onclick", OnClick);

            builder.AddContent(7, ChildContent);

            builder.CloseElement();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await Task.Delay(TimeSpan.FromMilliseconds(100.0d));
            }

            return base.OnAfterRenderAsync(firstRender);
        }
    }
}