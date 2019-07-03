using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class IdentityApiOptions : ApiOptionsBase
    {
        private string clientId;
        private string address;
        private string redirectUri;
        private IEnumerable<string> scopes;
        private Uri host;


        public override Uri Host
        {
            get => host;
            set
            {
                if (Uri.Equals(host, value))
                {
                    return;
                }

                host = value;
            }
        }

        public string Address
        {
            get => address;
            set
            {
                if (value == address)
                {
                    return;
                }

                address = value;
            }
        }

        public string ClientId
        {
            get => clientId;
            set
            {
                if (String.Equals(clientId, value))
                {
                    return;
                }

                clientId = value;
            }
        }

        public IEnumerable<string> Scopes
        {
            get => scopes;
            set
            {
                if (ReferenceEquals(scopes, value))
                {
                    return;
                }

                scopes = value;
            }
        }

        public string RedirectUri
        {
            get => redirectUri;
            set
            {
                if (ReferenceEquals(redirectUri, value))
                {
                    return;
                }

                redirectUri = value;
            }
        }

        public IdentityApiOptions()
        {
        }
    }
}