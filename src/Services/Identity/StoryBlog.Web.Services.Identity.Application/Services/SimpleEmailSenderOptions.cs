using System.Net;

namespace StoryBlog.Web.Services.Identity.Application.Services
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