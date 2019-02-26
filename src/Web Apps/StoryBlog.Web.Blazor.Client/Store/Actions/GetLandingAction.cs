using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Includes;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    public sealed class GetLandingAction : IAction
    {
        public LandingIncludes Includes
        {
            get;
        }

        public GetLandingAction(LandingIncludes includes)
        {
            Includes = includes;
        }
    }
}