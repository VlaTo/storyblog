using System.Net.Mail;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MailMessageTemplate
    {
        protected MailMessageTemplate()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Task<MailMessage> GenerateAsync(MailMessageTemplateContext context);
    }
}