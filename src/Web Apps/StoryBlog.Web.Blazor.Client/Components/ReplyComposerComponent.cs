using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace StoryBlog.Web.Blazor.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public class ReplyComposerComponent : ComponentBase
    {
        protected readonly EventCallback<MouseEventArgs> onClick;

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
            onClick = EventCallback.Factory.Create<MouseEventArgs>(this, DoClick);
        }

        private Task DoClick(MouseEventArgs e) => OnSubmit.InvokeAsync(this);
    }
}