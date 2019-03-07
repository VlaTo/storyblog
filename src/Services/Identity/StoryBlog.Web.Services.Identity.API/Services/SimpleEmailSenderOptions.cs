using System.Net;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    public sealed class SimpleEmailSenderOptions
    {
        public ICredentials Credentials
        {
            get;
            set;
        }
    }
}