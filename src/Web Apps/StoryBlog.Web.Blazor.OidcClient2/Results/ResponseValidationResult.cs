using System.Security.Claims;
using StoryBlog.Web.Blazor.OidcClient2.Messages;

namespace StoryBlog.Web.Blazor.OidcClient2.Results
{
    internal class ResponseValidationResult : Result
    {
        public ResponseValidationResult()
        {

        }

        public ResponseValidationResult(string error)
        {
            Error = error;
        }

        public virtual AuthorizeResponse AuthorizeResponse
        {
            get;
            set;
        }

        public virtual TokenResponse TokenResponse
        {
            get;
            set;
        }

        public virtual ClaimsPrincipal User
        {
            get;
            set;
        }
    }
}