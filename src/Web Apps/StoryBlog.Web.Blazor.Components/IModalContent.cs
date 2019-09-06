using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Components
{
    public interface IModalContent
    {
        string Title
        {
            get;
        }

        RenderFragment Content
        {
            get;
        }

        void OnCallback();
    }
}