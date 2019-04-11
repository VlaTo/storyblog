using System.Security.Principal;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task SigninAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IPrincipal> SigninCallbackAsync();
    }
}