using System;
using System.Net.Http;
using StoryBlog.Web.Blazor.Client.OidcClient.Messages;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Internal
{
    internal static class ClientCredentialsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="httpRequest"></param>
        internal static void PopulateClientCredentials(Request request, HttpRequestMessage httpRequest)
        {
            if (false == String.IsNullOrWhiteSpace(request.ClientId))
            {
                if (ClientCredentialStyle.AuthorizationHeader == request.ClientCredentialStyle)
                {
                    switch (request.AuthorizationHeaderStyle)
                    {
                        case BasicAuthenticationHeaderStyle.Rfc6749:
                        {
                            httpRequest.SetBasicAuthenticationOAuth(request.ClientId, request.ClientSecret ?? "");
                            break;
                        }

                        case BasicAuthenticationHeaderStyle.Rfc2617:
                        {
                            httpRequest.SetBasicAuthentication(request.ClientId, request.ClientSecret ?? "");
                            break;
                        }

                        default:
                        {
                            throw new InvalidOperationException("Unsupported basic authentication header style");
                        }
                    }
                }
                else if (request.ClientCredentialStyle == ClientCredentialStyle.PostBody)
                {
                    request.Parameters.AddRequired(OidcConstants.TokenRequest.ClientId, request.ClientId);
                    request.Parameters.AddOptional(OidcConstants.TokenRequest.ClientSecret, request.ClientSecret);
                }
                else
                {
                    throw new InvalidOperationException("Unsupported client credential style");
                }
            }

            request.Parameters.AddOptional(OidcConstants.TokenRequest.ClientAssertionType, request.ClientAssertion.Type);
            request.Parameters.AddOptional(OidcConstants.TokenRequest.ClientAssertion, request.ClientAssertion.Value);
        }
    }
}