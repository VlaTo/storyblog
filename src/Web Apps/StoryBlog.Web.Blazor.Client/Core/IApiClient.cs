using System;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Shared.Cart;

namespace StoryBlog.Web.Blazor.Client.Core
{
    public interface IApiClient
    {
        Task<Product[]> LoadCartProductsAsync(Guid cartId);
    }
}