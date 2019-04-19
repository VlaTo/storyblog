using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace StoryBlog.Web.Services.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ILoginService<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<SignInResult> ValidateCredentialsAsync(TEntity user, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<TEntity> FindByEmailAsync(string email);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="properties"></param>
        /// <param name="authenticationMethod"></param>
        /// <returns></returns>
        Task SigninAsync(TEntity user, AuthenticationProperties properties, string authenticationMethod);
    }
}