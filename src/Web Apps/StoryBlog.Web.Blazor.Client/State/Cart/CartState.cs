using System;
using StoryBlog.Web.Blazor.Shared;
using StoryBlog.Web.Blazor.Shared.Cart;

namespace StoryBlog.Web.Blazor.Client.State.Cart
{
    public sealed class CartState : IMutableState
    {
        public Guid Id
        {
            get;
        }

        public bool IsLoading
        {
            get;
        }

        public Product[] Products
        {
            get;
        }

        public string ErrorMessage
        {
            get;
        }

        private CartState(Guid id, bool isLoading, Product[] products, string errorMessage)
        {
            Id = id;
            IsLoading = isLoading;
            Products = products;
            ErrorMessage = errorMessage;
        }

        public static CartState Succeeded(Guid cartId, Product[] products)
        {
            return new CartState(cartId, false, products, null);
        }

        public static CartState Failed(Guid cartId, string errorMessage)
        {
            return new CartState(cartId, false, null, errorMessage);
        }

        public static CartState Loading(Guid cartId)
        {
            return new CartState(cartId, true, null, null);
        }

        public static CartState Empty()
        {
            return new CartState(Guid.Empty, false, null, null);
        }

        public int Version => GetHashCode();

        public void Apply(IMutator action)
        {
        }
    }
}