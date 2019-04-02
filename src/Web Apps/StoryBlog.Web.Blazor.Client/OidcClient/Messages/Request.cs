using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Messages
{
    /// <summary>
    /// Models a base OAuth/OIDC request with client credentials
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client assertion.
        /// </summary>
        /// <value>
        /// The assertion.
        /// </value>
        public ClientAssertion ClientAssertion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client credential style.
        /// </summary>
        /// <value>
        /// The client credential style.
        /// </value>
        public ClientCredentialStyle ClientCredentialStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the basic authentication header style.
        /// </summary>
        /// <value>
        /// The basic authentication header style.
        /// </value>
        public BasicAuthenticationHeaderStyle AuthorizationHeaderStyle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets optional parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public IDictionary<string, string> Parameters
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public Request()
        {
            Parameters = new Dictionary<string, string>();
            AuthorizationHeaderStyle = BasicAuthenticationHeaderStyle.Rfc6749;
            ClientCredentialStyle = ClientCredentialStyle.PostBody;
            ClientAssertion = new ClientAssertion();
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Request Clone()
        {
            var clone = new Request
            {
                Address = Address,
                AuthorizationHeaderStyle = AuthorizationHeaderStyle,
                ClientAssertion = ClientAssertion,
                ClientCredentialStyle = ClientCredentialStyle,
                ClientId = ClientId,
                ClientSecret = ClientSecret
            };

            if (null != Parameters)
            {
                foreach (var kvp in Parameters)
                {
                    clone.Parameters.Add(kvp);
                }
            }

            return clone;
        }
    }
}