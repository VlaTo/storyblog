﻿using System.Net.Mail;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISimpleEmailSender
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessageAsync(MailMessage message);
    }
}