using System;
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
        protected bool IsActive
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

        protected string ClassString
        {
            get;
            private set;
        }

        public BootstrapButtonComponent()
        {
            Type = BootstrapButtonTypes.Default;
        }

        static BootstrapButtonComponent()
        {
            classNameBuilder = new ClassBuilder<BootstrapButtonComponent>("btn", null)
                .DefineClass(
                    component => EnumHelper.GetClassName(component.Type),
                    component => BootstrapButtonTypes.Default != component.Type
                )
                .DefineClass("lg", component => BootstrapButtonSizes.Large == component.Size)
                .DefineClass("sm", component => BootstrapButtonSizes.Small == component.Size)
                .DefineClass("active", component => component.IsActive, PrefixSeparators.Dash);
        }

        protected override void OnInit()
        {
            ClassString = classNameBuilder.Build(this, Class);
        }
    }
}