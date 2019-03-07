using System.Net.Mail;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SimpleMailMessageTemplate : MailMessageTemplate
    {
        /// <inheritdoc />
        public override Task<MailMessage> GenerateAsync(MailMessageTemplateContext context)
        {
            var message = new MailMessage
            {
                From = context.From,
                Subject = context.Subject
            };

            foreach (var address in context.To)
            {
                message.To.Add(address);
            }

            return Task.FromResult(message);
        }
    }
}