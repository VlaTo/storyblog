using System.Diagnostics;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using System.Net.Http;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninActionEffect : Effect<SigninAction>
    {
        private readonly IUserApiClient client;

        public SigninActionEffect(IUserApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="Effect{TTriggerAction}.HandleAsync" />
        protected override async Task HandleAsync(SigninAction action, IDispatcher dispatcher)
        {
            try
            {
                await client.LoginAsync();
            }
            catch (HttpRequestException exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninCallbackActionEffect : Effect<SigninCallbackAction>
    {
        private readonly IUserApiClient client;

        public SigninCallbackActionEffect(IUserApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="Effect{TTriggerAction}.HandleAsync" />
        protected override async Task HandleAsync(SigninCallbackAction action, IDispatcher dispatcher)
        {
            try
            {
                var principal = await client.SigninCallbackAsync();
                dispatcher.Dispatch(new SigninCallbackSuccessAction(principal));
            }
            catch (HttpRequestException exception)
            {
                Debug.WriteLine(exception);
                throw;
            }
        }
    }
}