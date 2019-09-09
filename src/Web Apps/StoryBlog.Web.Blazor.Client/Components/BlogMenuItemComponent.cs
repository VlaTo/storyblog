using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using StoryBlog.Web.Blazor.Components;

namespace StoryBlog.Web.Blazor.Client.Components
{
    public class BlogMenuItemComponent : BootstrapComponentBase
    {
        private static readonly ClassBuilder<BlogMenuItemComponent> classNameBuilder;

        [Parameter]
        public bool IsBlock
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
        public string Title
        {
            get;
            set;
        }

        [Parameter]
        public string Link
        {
            get;
            set;
        }

        [Inject]
        protected NavigationManager NavigationManager
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

        protected override void OnInitialized()
        {
            RefreshStyles();
            ClassString = classNameBuilder.Build(this, Class);
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //await Task.Delay(TimeSpan.FromMilliseconds(100.0d));
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        protected override void OnDispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }

        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            RefreshStyles();
            StateHasChanged();
        }

        private void RefreshStyles()
        {
            var uri = new Uri(NavigationManager.Uri);
            var path = uri.LocalPath;
            //var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            IsActive = String.Equals(Link, path, StringComparison.InvariantCultureIgnoreCase);
            ClassString = classNameBuilder.Build(this, Class);
        }
    }
}