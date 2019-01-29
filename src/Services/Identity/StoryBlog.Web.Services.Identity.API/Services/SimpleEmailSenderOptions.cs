using System.Net;
using System.Net.Mail;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    public sealed class SimpleEmailSenderOptions
    {
        public ICredentials Credentials
        {
            get;
            set;
        }

        public MailAddress From
        {
            get;
            set;
        }
    }
}