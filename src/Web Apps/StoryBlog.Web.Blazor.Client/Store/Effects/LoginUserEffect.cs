using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using System.Net.Http;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    public sealed class LoginUserEffect : Effect<LoginAction>
    {
        private readonly IUserApiClient client;

        public LoginUserEffect(IUserApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(LoginAction action, IDispatcher dispatcher)
        {
            try
            {
                await client.LoginAsync();
            }
            catch (HttpRequestException exception)
            {
                ;
            }
        }
    }
}