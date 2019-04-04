using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Services.Identity.API.Data.Models;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    public sealed class EntityFrameworkLoginService : ILoginService<Customer>
    {
        private readonly UserManager<Customer> userManager;
        private readonly SignInManager<Customer> signInManager;

        public EntityFrameworkLoginService(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<SignInResult> ValidateCredentialsAsync(Customer user, string password)
        {
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
            return result;
        }

        public Task<Customer> FindByUsernameAsync(string username)
        {
            return userManager.FindByNameAsync(username);
        }

        public Task SigninAsync(Customer user, AuthenticationProperties properties)
        {
            return signInManager.SignInAsync(user, properties);
        }
    }
}