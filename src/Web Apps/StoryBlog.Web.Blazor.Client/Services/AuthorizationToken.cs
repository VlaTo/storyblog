namespace StoryBlog.Web.Blazor.Client.Services
{
    public class AuthorizationToken
    {
        public string Scheme
        {
            get;
        }

        public string Payload
        {
            get;
        }

        public AuthorizationToken(string scheme, string payload)
        {
            Scheme = scheme;
            Payload = payload;
        }
    }
}