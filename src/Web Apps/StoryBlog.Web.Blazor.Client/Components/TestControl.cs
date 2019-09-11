using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Client.Components
{
    [Route("/TestControl/{number}")]
    public class TestControl : IComponent
    {
        [Inject]
        public NavigationManager NavigationManager
        {
            get;
            set;
        }

        [Parameter]
        public int Number
        {
            get;
            set;
        }


        void IComponent.Attach(RenderHandle renderHandle)
        {
        }

        Task IComponent.SetParametersAsync(ParameterView parameters)
        {
            return Task.CompletedTask;
        }
    }
}