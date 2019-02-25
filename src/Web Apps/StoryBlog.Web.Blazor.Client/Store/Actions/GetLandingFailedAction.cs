using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    public sealed class GetLandingFailedAction : IAction
    {
        public string Error
        {
            get;
        }

        public GetLandingFailedAction(string error)
        {
            Error = error;
        }
    }
}