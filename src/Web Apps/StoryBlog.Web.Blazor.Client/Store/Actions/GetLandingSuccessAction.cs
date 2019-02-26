using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    public sealed class GetLandingSuccessAction : IAction
    {
        public LandingModel Data
        {
            get;
        }

        public GetLandingSuccessAction(LandingModel data)
        {
            Data = data;
        }
    }
}