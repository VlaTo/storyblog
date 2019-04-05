using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninAction : IAction
    {
        public SigninAction()
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninCallbackAction : IAction
    {
        public SigninCallbackAction()
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninCallbackSuccessAction : IAction
    {
        public IPrincipal Principal
        {
            get;
        }

        public SigninCallbackSuccessAction(IPrincipal principal)
        {
            Principal = principal;
        }
    }
}