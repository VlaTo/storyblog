using System.Collections.Generic;
using System.Security.Claims;
using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    public sealed class LoginAction : IAction
    {
        public LoginAction()
        {
        }
    }

    public sealed class LoginSuccessAction : IAction
    {
        public string Token
        {
            get;
        }

        public IEnumerable<Claim> Claims
        {
            get;
        }

        public LoginSuccessAction(string token, IEnumerable<Claim> claims)
        {
            Token = token;
            Claims = claims;
        }
    }

    public sealed class GetUserInfoAction : IAction
    {
        public string Token
        {
            get;
        }

        public GetUserInfoAction(string token)
        {
            Token = token;
        }
    }
}