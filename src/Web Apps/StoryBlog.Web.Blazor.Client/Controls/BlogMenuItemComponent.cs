﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Services;
using StoryBlog.Web.Blazor.Components;
using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Controls
{
    public class BlogMenuItemComponent : BootstrapComponentBase
    {
        private static readonly ClassBuilder<BlogMenuItemComponent> classNameBuilder;

        private bool firstRender;

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

        [Inject]
        protected IUriHelper UriHelper
        {
            get;
            set;
        }

        protected string ClassString
        {
            get;
            private set;
        }

        protected bool IsActive
        {
            get;
            private set;
        }

        public BlogMenuItemComponent()
        {
            firstRender = true;
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
            RefreshStyles();
            ClassString = classNameBuilder.Build(this, Class);
            UriHelper.OnLocationChanged += OnLocationChanged;
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

        protected override void OnDispose()
        {
            UriHelper.OnLocationChanged -= OnLocationChanged;
        }

        private void OnLocationChanged(object sender, string e)
        {
            RefreshStyles();
            StateHasChanged();
        }

        private void RefreshStyles()
        {
            var uri = new Uri(UriHelper.GetAbsoluteUri());
            var path = uri.LocalPath;
            //var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            if (String.Equals(Link, path, StringComparison.InvariantCultureIgnoreCase))
            {
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }

            ClassString = classNameBuilder.Build(this, Class);
        }
    }
}