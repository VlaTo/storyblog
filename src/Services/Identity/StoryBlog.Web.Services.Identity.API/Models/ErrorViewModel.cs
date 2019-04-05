using IdentityServer4.Models;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    public sealed class ErrorViewModel
    {
        public ErrorMessage Error
        {
            get;
        }

        public ErrorViewModel(ErrorMessage error)
        {
            Error = error;
        }
    }
}