﻿using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SimpleEmailTemplateGenerator : IEmailTemplateGenerator
    {
        private readonly ILogger<SimpleEmailTemplateGenerator> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public SimpleEmailTemplateGenerator(ILogger<SimpleEmailTemplateGenerator> logger)
        {
            this.logger = logger;
        }

        public Task<MailMessageTemplate> ResolveTemplateAsync(string key)
        {
            logger.LogDebug($"Resolving email message template for key: \'{key}\'");

            return Task.FromResult<MailMessageTemplate>(new SimpleMailMessageTemplate());
        }
    }
}