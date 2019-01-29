using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    public interface ILoginService<TEntity>
    {
        Task<SignInResult> ValidateCredentialsAsync(TEntity user, string password);

        Task<TEntity> FindByUsernameAsync(string username);

        Task SigninAsync(TEntity user, AuthenticationProperties properties);
    }
}