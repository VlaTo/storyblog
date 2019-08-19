using Blazor.Fluxor;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentBoxComponent : ComponentBase
    {
        [Inject]
        protected IDispatcher Dispatcher
        {
            get;
            set;
        }

        [Inject]
        protected IPluralLocalizer Pluralizer
        {
            get;
            set;
        }

        [Parameter]
        public long? ParentId
        {
            get;
            set;
        }

        [Parameter]
        public string Message
        {
            get;
            set;
        }

        [Parameter]
        public EventCallback<string> OnSubmit
        {
            get;
            set;
        }

        protected void OnSubmitButtonClick(UIMouseEventArgs e)
        {
            OnSubmit.InvokeAsync(Message);
        }
    }
}