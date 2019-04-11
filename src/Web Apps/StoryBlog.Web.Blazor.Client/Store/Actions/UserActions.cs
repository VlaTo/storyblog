using Blazor.Fluxor;
using System.Security.Principal;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestAction : IAction
    {
        public SigninRequestAction()
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestFailedAction : IAction
    {
        public string Error
        {
            get;
        }

        public SigninRequestFailedAction(string error)
        {
            Error = error;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestSuccessAction : IAction
    {
        public IPrincipal Principal
        {
            get;
        }

        public SigninRequestSuccessAction(IPrincipal principal)
        {
            Principal = principal;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestCallbackFailedAction : IAction
    {
        public string Error
        {
            get;
        }

        public SigninRequestCallbackFailedAction(string error)
        {
            Error = error;
        }
    }
}