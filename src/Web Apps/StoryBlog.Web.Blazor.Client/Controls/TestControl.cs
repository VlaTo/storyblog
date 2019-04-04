using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Services;

namespace StoryBlog.Web.Blazor.Client.Controls
{
    [Route("/TestControl/{number}")]
    public class TestControl : IComponent
    {
        [Inject]
        public IUriHelper UrlHelper
        {
            get;
            set;
        }

        public int Number
        {
            get;
            set;
        }

        public void Configure(RenderHandle renderHandle)
        {
            throw new System.NotImplementedException();
        }

        public Task SetParametersAsync(ParameterCollection parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}