using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    public class LoginAction : IAction
    {
        public LoginAction()
        {
        }
    }

    public class LoginSuccessAction : IAction
    {
        public string Token
        {
            get;
        }

        public LoginSuccessAction(string token)
        {
            Token = token;
        }
    }
}