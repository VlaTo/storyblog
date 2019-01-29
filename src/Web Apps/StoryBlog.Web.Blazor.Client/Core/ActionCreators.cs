using System;
using System.Threading.Tasks;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.State.Cart;

namespace StoryBlog.Web.Blazor.Client.Core
{
    public static class ActionCreators
    {
        public static async Task LoadCartProductsAsync(IDispatcher dispatcher, IApiClient client, Guid cartId)
        {
            try
            {
                dispatcher.Dispatch(new GetCartStateAction(cartId));

                var products = await client.LoadCartProductsAsync(cartId);

                dispatcher.Dispatch(new GetCartStateSucceededAction(cartId, products));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetCartStateFailedAction(cartId, exception.Message));
            }
        }
    }
}