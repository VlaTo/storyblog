using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Services.Identity.API.Data.Models;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityFrameworkLoginService : ILoginService<Customer>
    {
        private readonly UserManager<Customer> userManager;
        private readonly SignInManager<Customer> signInManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public EntityFrameworkLoginService(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <inheritdoc cref="ILoginService{TEntity}.ValidateCredentialsAsync" />
        public async Task<SignInResult> ValidateCredentialsAsync(Customer user, string password)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            return result;
        }

        /// <inheritdoc cref="ILoginService{TEntity}.FindByEmailAsync" />
        public Task<Customer> FindByEmailAsync(string email)
        {
            return userManager.FindByEmailAsync(email);
        }

        /// <inheritdoc cref="ILoginService{TEntity}.SigninAsync" />
        public Task SigninAsync(Customer user, AuthenticationProperties properties, string authenticationMethod)
        {
            return signInManager.SignInAsync(user, properties, authenticationMethod);
        }
    }
}