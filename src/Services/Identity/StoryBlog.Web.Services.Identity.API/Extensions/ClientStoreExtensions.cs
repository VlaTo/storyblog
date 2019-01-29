using System;
using System.Threading.Tasks;
using IdentityServer4.Stores;

namespace StoryBlog.Web.Services.Identity.API.Extensions
{
    public static class ClientStoreExtensions
    {
        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string clientId)
        {
            if (String.IsNullOrWhiteSpace(clientId))
            {
                return false;
            }

            var client = await store.FindEnabledClientByIdAsync(clientId);

            return true == client?.RequirePkce;
        }
    }
}