using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    public class SimpleEmailSender : IEmailSender
    {
        private readonly SimpleEmailSenderOptions options;

        public SimpleEmailSender(IOptionsSnapshot<SimpleEmailSenderOptions> snapshot)
        {
            if (null == snapshot)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            options = snapshot.Value;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}