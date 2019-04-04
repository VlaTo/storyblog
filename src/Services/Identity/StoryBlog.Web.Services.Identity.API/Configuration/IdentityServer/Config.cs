using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace StoryBlog.Web.Services.Identity.API.Configuration.IdentityServer
{
    internal static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("api.identity", "StoryBlog Identity API"),
                new ApiResource("api.blog", "StoryBlog Blog API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials client
                new Client
                {
                    ClientId = "client.application",
                    ClientName = "Blazor Web application",
                    AllowedGrantTypes = GrantTypes.Code,
                    AccessTokenType = AccessTokenType.Jwt,
                    RequireClientSecret = false,
                    RequirePkce = true,

                    AllowedCorsOrigins = {"http://localhost:62742"},
                    RedirectUris = {"http://localhost:62742/callback"},
                    PostLogoutRedirectUris = {"http://localhost:62742/index"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.identity",
                        "api.blog"
                    }
                }
            };
        }
    }
}