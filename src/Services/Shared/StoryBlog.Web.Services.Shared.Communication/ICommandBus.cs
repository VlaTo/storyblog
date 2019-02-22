using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.Communication
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : IntegrationCommand;
    }
}