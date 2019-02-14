using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    public sealed class GetStoriesListFailedAction : IAction
    {
        public string Error
        {
            get;
        }

        public GetStoriesListFailedAction(string error)
        {
            Error = error;
        }
    }
}