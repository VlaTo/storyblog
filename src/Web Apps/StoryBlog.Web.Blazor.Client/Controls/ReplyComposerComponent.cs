using System;
using System.Threading.Tasks;
using Blazor.Fluxor;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ReplyComposerComponent : ComponentBase
    {
        protected readonly EventCallback<UIMouseEventArgs> onClick;

        [Parameter]
        public string Message
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback OnSubmit
        {
            get;
            set;
        }

        public ReplyComposerComponent()
        {
            onClick = EventCallback.Factory.Create<UIMouseEventArgs>(this, DoClick);
        }

        private Task DoClick(UIMouseEventArgs e) => OnSubmit.InvokeAsync(this);
    }
}