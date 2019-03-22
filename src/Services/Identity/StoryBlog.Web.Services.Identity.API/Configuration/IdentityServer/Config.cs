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
                }),
                new ApiResource("api.blog", "StoryBlog Blog API", new[]
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
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedCorsOrigins =
                    {
                        "http://localhost:3000"
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.identity",
                        "api.blog"
                    },

                    RedirectUris =
                    {
                        "http://localhost:5100/#callback"
                    },

                    AccessTokenLifetime = 300,
                    IdentityTokenLifetime = 3600,
                    AllowOfflineAccess = false
                },

                // client credentials client
                new Client
                {
                    ClientId = "api.blog",
                    ClientName = "Blog API",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AccessTokenType = AccessTokenType.Jwt,
                    //AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    AlwaysIncludeUserClaimsInIdToken = true,

                    AllowedCorsOrigins =
                    {
                        "http://localhost:3100/",
                        "http://localhost:64972/"
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "api.identity",
                        "api.blog"
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