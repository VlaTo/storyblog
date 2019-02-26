using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    public sealed class LandingState
    {
        public bool IsBusy
        {
            get;
        }

        public LandingModel Data
        {
            get;
        }

        public string Error
        {
            get;
        }

        public LandingState(bool isBusy, LandingModel data, string error)
        {
            IsBusy = isBusy;
            Data = data;
            Error = error;
        }
    }
}