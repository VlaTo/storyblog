using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Client.OidcClient.Internal;
using StoryBlog.Web.Blazor.Client.OidcClient.Messages;

namespace StoryBlog.Web.Blazor.Client.OidcClient
{
    public static class HttpClientTokenRequestExtensions
    {
        /// <summary>
        /// Sends a token request using the client_credentials grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestClientCredentialsTokenAsync(
            this HttpMessageInvoker client,
            ClientCredentialsTokenRequest request,
            CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.ClientCredentials);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.Scope, request.Scope);

            return await client.RequestTokenAsync(clone, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Sends a token request using the authorization_code grant type.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestAuthorizationCodeTokenAsync(
            this HttpMessageInvoker client,
            AuthorizationCodeTokenRequest request,
            CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, OidcConstants.GrantTypes.AuthorizationCode);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.Code, request.Code);
            clone.Parameters.AddRequired(OidcConstants.TokenRequest.RedirectUri, request.RedirectUri);
            clone.Parameters.AddOptional(OidcConstants.TokenRequest.CodeVerifier, request.CodeVerifier);

            return await client.RequestTokenAsync(clone, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a token request.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public static async Task<TokenResponse> RequestTokenAsync(
            this HttpMessageInvoker client,
            TokenRequest request,
            CancellationToken cancellationToken = default)
        {
            var clone = request.Clone();

            if (false == clone.Parameters.ContainsKey(OidcConstants.TokenRequest.GrantType))
            {
                clone.Parameters.AddRequired(OidcConstants.TokenRequest.GrantType, request.GrantType);
            }

            return await client.RequestTokenAsync(clone, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<TokenResponse> RequestTokenAsync(
            this HttpMessageInvoker client,
            Request request,
            CancellationToken cancellationToken)
        {
            if (false == request.Parameters.TryGetValue(OidcConstants.TokenRequest.ClientId, out _))
            {
                if (String.IsNullOrWhiteSpace(request.ClientId))
                {
                    throw new InvalidOperationException("client_id is missing");
                }
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, request.Address);

            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ClientCredentialsHelper.PopulateClientCredentials(request, httpRequest);
            httpRequest.Content = new FormUrlEncodedContent(request.Parameters);

            HttpResponseMessage response;

            try
            {
                response = await client.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new TokenResponse(ex);
            }

            string content = null;

            if (null != response.Content)
            {
                content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            if (HttpStatusCode.OK == response.StatusCode || HttpStatusCode.BadRequest == response.StatusCode)
            {
                return new TokenResponse(content);
            }

            return new TokenResponse(response.StatusCode, response.ReasonPhrase, content);
        }
    }
}