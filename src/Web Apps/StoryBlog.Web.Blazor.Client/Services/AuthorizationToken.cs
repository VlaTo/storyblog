namespace StoryBlog.Web.Blazor.Client.Services
{
    public class AuthorizationToken
    {
        public string Token
        {
            get;
        }

        public AuthorizationToken(string token)
        {
            Token = token;
        }
    }
}
