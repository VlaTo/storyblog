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
                new ApiResource("api.identity", "StoryBlog Identity API", new[]
                {
                    "name", "role"
                })
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
                    //AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedCorsOrigins =
                    {
                        "http://localhost:5100"
                    },

                    /*ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },*/

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.identity"
                    },

                    RedirectUris =
                    {
                        "http://localhost:5100/#callback"
                    },

                    AccessTokenLifetime = 300,
                    IdentityTokenLifetime = 3600,
                    AllowOfflineAccess = false
                },

                //
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    /*ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },*/

                    /*RedirectUris =
                    {
                        "http://localhost:2606/signin-oid"
                    },

                    PostLogoutRedirectUris =
                    {
                        "http://localhost:2606/signout-callback-oid"
                    },*/

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api"
                    },

                    RequireClientSecret = false,
                    RequireConsent = false,
                    AllowOfflineAccess = true
                }
            };
        }
    }
}