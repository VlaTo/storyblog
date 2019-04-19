using Microsoft.Extensions.Options;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleEmailSender : ISimpleEmailSender
    {
        private readonly SimpleEmailSenderOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapshot"></param>
        public SimpleEmailSender(IOptions<SimpleEmailSenderOptions> snapshot)
        {
            if (null == snapshot)
            {
                throw new ArgumentNullException(nameof(snapshot));
            }

            options = snapshot.Value;
        }

        /// <inheritdoc />
        public async Task SendMessageAsync(MailMessage message)
        {
            if (null == message)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await Task.CompletedTask;
        }
    }
}