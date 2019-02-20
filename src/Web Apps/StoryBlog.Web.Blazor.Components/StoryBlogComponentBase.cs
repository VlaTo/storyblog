using Microsoft.AspNetCore.Blazor.Components;

namespace StoryBlog.Web.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class BootstrapComponentBase : BlazorComponent
    {
        [Parameter]
        protected string Class
        {
            get;
            set;
        }
    }
}
