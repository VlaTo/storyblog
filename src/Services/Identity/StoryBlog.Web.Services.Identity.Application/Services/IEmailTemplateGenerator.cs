using StoryBlog.Web.Services.Identity.Application.Models;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmailTemplateGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<MailMessageTemplate> ResolveTemplateAsync(string key);
    }
}