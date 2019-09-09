using System.Threading.Tasks;
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

        ModalButton[] Buttons
        {
            get;
        }

        Task<ModalButton> WaitForComplete();

        void SetResult(ModalButton button);
    }
}