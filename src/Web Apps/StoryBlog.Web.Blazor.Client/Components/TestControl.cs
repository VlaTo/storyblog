using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Components
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