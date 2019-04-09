using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Services
{
    public sealed class UserApiClientOptions
    {
        private string clientId;
        private Uri address;
        private IEnumerable<string> scopes;

        public Uri Address
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

        public UserApiClientOptions()
        {
        }
    }
}