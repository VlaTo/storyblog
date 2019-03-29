using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Blazor.OidcClient2.Browser;
using StoryBlog.Web.Blazor.OidcClient2.Infrastructure;
using StoryBlog.Web.Blazor.OidcClient2.Results;

namespace StoryBlog.Web.Blazor.OidcClient2
{
    internal sealed class AuthorizationClient
    {
        private readonly CryptoHelper _crypto;
        private readonly ILogger<AuthorizationClient> _logger;
        private readonly OidcClientOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationClient"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public AuthorizationClient(OidcClientOptions options)
        {
            _options = options;
            _logger = options.LoggerFactory.CreateLogger<AuthorizationClient>();
            _crypto = new CryptoHelper(options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayMode"></param>
        /// <param name="timeout"></param>
        /// <param name="extraParameters"></param>
        /// <returns></returns>
        public async Task<AuthorizeResult> AuthorizeAsync(
            DisplayMode displayMode = DisplayMode.Visible,
            int timeout = 300,
            object extraParameters = null)
        {
            _logger.LogTrace("AuthorizeAsync");

            if (_options.Browser == null)
            {
                throw new InvalidOperationException("No browser configured.");
            }

            var result = new AuthorizeResult
            {
                State = CreateAuthorizeState(extraParameters)
            };

            var browserOptions = new BrowserOptions(result.State.StartUrl, _options.RedirectUri)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                DisplayMode = displayMode,
                ResponseMode = AuthorizeResponseMode.FormPost == _options.ResponseMode
                    ? AuthorizeResponseMode.FormPost
                    : AuthorizeResponseMode.Redirect
            };

            var browserResult = await _options.Browser.InvokeAsync(browserOptions);

            if (BrowserResultType.Success == browserResult.ResultType)
            {
                result.Data = browserResult.Response;
                return result;
            }

            result.Error = browserResult.ResultType.ToString();

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extraParameters"></param>
        /// <returns></returns>
        public AuthorizeState CreateAuthorizeState(object extraParameters = null)
        {
            _logger.LogTrace("CreateAuthorizeStateAsync");

            var pkce = _crypto.CreatePkceData();
            var state = new AuthorizeState
            {
                Nonce = _crypto.CreateNonce(),
                State = _crypto.CreateState(),
                RedirectUri = _options.RedirectUri,
                CodeVerifier = pkce.CodeVerifier,
            };

            state.StartUrl = CreateAuthorizeUrl(state.State, state.Nonce, pkce.CodeChallenge, extraParameters);

            _logger.LogDebug(LogSerializer.Serialize(state));

            return state;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="nonce"></param>
        /// <param name="codeChallenge"></param>
        /// <param name="extraParameters"></param>
        /// <returns></returns>
        internal string CreateAuthorizeUrl(string state, string nonce, string codeChallenge, object extraParameters)
        {
            _logger.LogTrace("CreateAuthorizeUrl");

            var parameters = CreateAuthorizeParameters(state, nonce, codeChallenge, extraParameters);
            var request = new RequestUrl(_options.ProviderInformation.AuthorizeEndpoint);

            return request.Create(parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <param name="nonce"></param>
        /// <param name="codeChallenge"></param>
        /// <param name="extraParameters"></param>
        /// <returns></returns>
        internal Dictionary<string, string> CreateAuthorizeParameters(string state, string nonce, string codeChallenge, object extraParameters)
        {
            _logger.LogTrace("CreateAuthorizeParameters");

            string responseType = null;

            switch (_options.Flow)
            {
                case AuthenticationFlow.AuthorizationCode:
                {
                    responseType = OidcConstants.ResponseTypes.Code;
                    break;
                }

                case AuthenticationFlow.Hybrid:
                {
                    responseType = OidcConstants.ResponseTypes.CodeIdToken;
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException(nameof(_options.Flow), "Unsupported authentication flow");
                }
            }

            var parameters = new Dictionary<string, string>
            {
                { OidcConstants.AuthorizeRequest.ResponseType, responseType },
                { OidcConstants.AuthorizeRequest.Nonce, nonce },
                { OidcConstants.AuthorizeRequest.State, state },
                { OidcConstants.AuthorizeRequest.CodeChallenge, codeChallenge },
                { OidcConstants.AuthorizeRequest.CodeChallengeMethod, OidcConstants.CodeChallengeMethods.Sha256 },
            };

            if (false == String.IsNullOrWhiteSpace(_options.ClientId))
            {
                parameters.Add(OidcConstants.AuthorizeRequest.ClientId, _options.ClientId);
            }

            if (false == String.IsNullOrWhiteSpace(_options.Scope))
            {
                parameters.Add(OidcConstants.AuthorizeRequest.Scope, _options.Scope);
            }

            if (false == String.IsNullOrWhiteSpace(_options.RedirectUri))
            {
                parameters.Add(OidcConstants.AuthorizeRequest.RedirectUri, _options.RedirectUri);
            }

            if (AuthorizeResponseMode.FormPost == _options.ResponseMode)
            {
                parameters.Add(OidcConstants.AuthorizeRequest.ResponseMode, OidcConstants.ResponseModes.FormPost);
            }

            var extraDictionary = ObjectToDictionary(extraParameters);

            if (null != extraDictionary)
            {
                foreach (var entry in extraDictionary)
                {
                    if (!string.IsNullOrWhiteSpace(entry.Value))
                    {
                        if (parameters.ContainsKey(entry.Key))
                        {
                            parameters[entry.Key] = entry.Value;
                        }
                        else
                        {
                            parameters.Add(entry.Key, entry.Value);
                        }
                    }
                }
            }

            return parameters;
        }

        private Dictionary<string, string> ObjectToDictionary(object values)
        {
            _logger.LogTrace("ObjectToDictionary");

            if (null == values)
            {
                return null;
            }

            if (values is Dictionary<string, string> dictionary)
            {
                return dictionary;
            }

            dictionary = new Dictionary<string, string>();

            foreach (var property in values.GetType().GetRuntimeProperties())
            {
                var value = property.GetValue(values) as string;

                if (String.IsNullOrEmpty(value))
                {
                    continue;
                }

                dictionary.Add(property.Name, value);
            }

            return dictionary;
        }
    }
}