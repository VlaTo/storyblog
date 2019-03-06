﻿using System;
using StoryBlog.Web.Blazor.Components;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor.Components;

namespace StoryBlog.Web.Blazor.Client.Controls
{
    public class BlogMenuItemComponent : BootstrapComponentBase
    {
        private static readonly ClassBuilder<BlogMenuItemComponent> classNameBuilder;

        private bool firstRender;

        [Parameter]
        protected bool IsActive
        {
            get;
            set;
        }

        [Parameter]
        protected bool IsBlock
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
        protected string Title
        {
            get;
            set;
        }

        [Parameter]
        protected string Link
        {
            get;
            set;
        }

        protected string ClassString
        {
            get;
            private set;
        }

        public BlogMenuItemComponent()
        {
            firstRender = true;

            IsActive = false;
            IsBlock = false;
        }

        static BlogMenuItemComponent()
        {
            classNameBuilder = new ClassBuilder<BlogMenuItemComponent>("menu-item", "item")
                /*.DefineClass(@class => @class
                    .Modifier("outline", component => component.IsOutline)
                    .Name(component => EnumHelper.GetClassName(component.Type))
                    .Condition(component => BootstrapButtonTypes.Default != component.Type)
                )*/
                .DefineClass(@class => @class.Name("block").Condition(component => component.IsBlock))
                //.DefineClass(@class => @class.Name("lg").Condition(component => BootstrapButtonSizes.Large == component.Size))
                //.DefineClass(@class => @class.Name("sm").Condition(component => BootstrapButtonSizes.Small == component.Size))
                .DefineClass(@class => @class.NoPrefix().Name("active").Condition(component => component.IsActive));
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
