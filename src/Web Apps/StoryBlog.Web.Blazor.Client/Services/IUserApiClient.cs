using System.Collections.Generic;
using System.Security.Claims;
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
        Task<string> LoginAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Claim>> GetUserInfoAsync(string token);
    }
}