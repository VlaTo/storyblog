using System;
using Microsoft.Extensions.Logging;

namespace StoryBlog.Web.Blazor.OidcClient2
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class OidcClient
    {
        private readonly ILogger _logger;
        private readonly AuthorizationClient _authorizeClient;

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public OidcClientOptions Options
        {
            get;
        }

        public OidcClient(OidcClientOptions options)
        {
            if (null == options)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (null == options.ProviderInformation)
            {
                if (String.IsNullOrWhiteSpace(options.Authority))
                {
                    throw new ArgumentException("No authority specified", nameof(options.Authority));
                }

                useDiscovery = true;
            }

            Options = options;
            _logger = options.LoggerFactory.CreateLogger<OidcClient>();
            _authorizeClient = new AuthorizationClient(options);
            _processor = new ResponseProcessor(options, EnsureProviderInformationAsync);
        }
    }
}