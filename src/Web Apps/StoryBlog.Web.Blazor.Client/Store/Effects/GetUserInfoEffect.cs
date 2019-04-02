using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    public class GetUserInfoEffect : Effect<GetUserInfoAction>
    {
        private readonly IUserApiClient client;

        public GetUserInfoEffect(IUserApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(GetUserInfoAction action, IDispatcher dispatcher)
        {
            try
            {
                // action.Token
                var claims = await client.GetUserInfoAsync(action.Token);
                dispatcher.Dispatch(new LoginSuccessAction(action.Token, claims));
            }
            catch (HttpRequestException exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }
    }
}