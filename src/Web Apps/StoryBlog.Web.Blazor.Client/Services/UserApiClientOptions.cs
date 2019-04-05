using System;

namespace StoryBlog.Web.Blazor.Client.Services
{
    public sealed class UserApiClientOptions
    {
        public Uri Address
        {
            get;
            set;
        }

        public string ClientId
        {
            get;
            set;
        }
    }
}