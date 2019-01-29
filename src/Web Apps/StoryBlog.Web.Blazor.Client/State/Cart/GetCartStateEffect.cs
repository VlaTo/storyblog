using System.Threading.Tasks;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Core;

namespace StoryBlog.Web.Blazor.Client.State.Cart
{
    public sealed class GetCartStateEffect : Effect<GetCartStateAction>
    {
        private readonly IApiClient apiClient;

        public GetCartStateEffect(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        protected override Task HandleAsync(GetCartStateAction action, IDispatcher dispatcher)
        {
            return ActionCreators.LoadCartProductsAsync(dispatcher, apiClient, action.CartId);
        }
    }
}