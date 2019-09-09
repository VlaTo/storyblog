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

        public int Number
        {
            get;
            set;
        }

        public void Attach(RenderHandle renderHandle)
        {
            throw new System.NotImplementedException();
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}