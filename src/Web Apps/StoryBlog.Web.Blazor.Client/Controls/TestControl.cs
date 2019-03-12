using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;

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

        public void Init(RenderHandle renderHandle)
        {
            throw new System.NotImplementedException();
        }

        public void SetParameters(ParameterCollection parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}