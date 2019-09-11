using Microsoft.AspNetCore.Components;
using System;

namespace StoryBlog.Web.Blazor.Client.Components
{
    public class FullPageSpinnerComponent : BootstrapComponentBase
    {
        private static readonly ClassBuilder<FullPageSpinnerComponent> classNameBuilder;

        [Parameter]
        public bool IsCentered
        {
            get; 
            set;
        }

        [Parameter]
        public bool IsActive
        {
            get; 
            set;
        }

        protected string ClassString
        {
            get;
            private set;
        }

        static FullPageSpinnerComponent()
        {
            classNameBuilder = new ClassBuilder<FullPageSpinnerComponent>(String.Empty)
                /*.DefineClass(@class => @class
                    .Modifier("outline", component => component.IsOutline)
                    .Name(component => EnumHelper.GetClassName(component.Type))
                    .Condition(component => BootstrapButtonTypes.Default != component.Type)
                )*/
                .DefineClass(@class=>@class.NoPrefix().Name("modal"))
                .DefineClass(@class=>@class.NoPrefix().Name("fade"))
                .DefineClass(@class => @class.NoPrefix().Name("show").Condition(component => component.IsActive))
                //.DefineClass(@class => @class.Name("lg").Condition(component => BootstrapButtonSizes.Large == component.Size))
                //.DefineClass(@class => @class.Name("sm").Condition(component => BootstrapButtonSizes.Small == component.Size))
                //.DefineClass(@class => @class.NoPrefix().Name("active").Condition(component => component.IsActive))
                ;
        }

        protected override void OnInitialized()
        {
            ClassString = classNameBuilder.Build(this, Class);
        }

        protected override bool ShouldRender()
        {
            if (base.ShouldRender())
            {
                ClassString = classNameBuilder.Build(this, Class);
                return true;
            }

            return false;
        }
    }
}