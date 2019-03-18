using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using StoryBlog.Web.Blazor.Components.Attributes;

namespace StoryBlog.Web.Blazor.Components
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
    public class BootstrapButtonComponent : BootstrapComponentBase
    {
        private static readonly ClassBuilder<BootstrapButtonComponent> classNameBuilder;

        private bool firstRender;
        protected ElementRef button;

        [Parameter]
        protected BootstrapButtonTypes Type
        {
            get;
            set;
        }

        [Parameter]
        protected BootstrapButtonSizes Size
        {
            get;
            set;
        }

        [Parameter]
        protected bool Active
        {
            get;
            set;
        }

        [Parameter]
        protected bool Outline
        {
            get;
            set;
        }

        [Parameter]
        protected bool Block
        {
            get;
            set;
        }

        [Parameter]
        protected bool Disabled
        {
            get;
            set;
        }

        [Parameter]
        protected Action<UIMouseEventArgs> OnClick
        {
            get;
            set;
        }

        [Parameter]
        protected RenderFragment ChildContent
        {
            get;
            set;
        }

        [Parameter]
        protected string Style
        {
            get;
            set;
        }

        [Parameter]
        protected int TabIndex
        {
            get;
            set;
        }

        protected string ClassString
        {
            get;
            private set;
        }

        public BootstrapButtonComponent()
        {
            firstRender = true;
            Type = BootstrapButtonTypes.Default;
            Size = BootstrapButtonSizes.Default;
            Outline = false;
            Active = false;
            Block = false;
            Disabled = false;
            TabIndex = 0;
        }

        static BootstrapButtonComponent()
        {
            classNameBuilder = new ClassBuilder<BootstrapButtonComponent>("btn")
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

        protected override void OnInit()
        {
            ClassString = classNameBuilder.Build(this, Class);
        }

        protected override Task OnAfterRenderAsync()
        {
            if (firstRender)
            {
                firstRender = false;
                //await Task.Delay(TimeSpan.FromMilliseconds(100.0d));
            }

            return base.OnAfterRenderAsync();
        }
    }
}