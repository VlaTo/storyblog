namespace StoryBlog.Web.Blazor.OidcClient2.Results
{
    internal class AuthorizeResult : Result
    {
        public virtual string Data
        {
            get;
            set;
        }

        public virtual AuthorizeState State
        {
            get;
            set;
        }
    }
}