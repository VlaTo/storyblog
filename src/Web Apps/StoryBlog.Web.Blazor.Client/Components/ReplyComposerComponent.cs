using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Client.Components
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