using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NullCommandBus : ICommandBus
    {
        private readonly ILogger<ICommandBus> logger;

        public NullCommandBus(ILogger<ICommandBus> logger)
        {
            this.logger = logger;
        }

        public Task SendAsync<TCommand>(TCommand command)
            where TCommand : IntegrationCommand
        {
            if (null == command)
            {
                throw new ArgumentNullException(nameof(command));
            }

            logger.LogDebug($"[NullCommandBus.SendAsync] command id: {command.Id} Sent: {command.Sent}");

            return Task.CompletedTask;
        }
    }
}