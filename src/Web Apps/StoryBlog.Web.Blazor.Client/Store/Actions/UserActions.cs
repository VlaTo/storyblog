using System.Security.Principal;

namespace StoryBlog.Web.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestAction
    {
        public SigninRequestAction()
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestFailedAction
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
    public sealed class SigninRequestSuccessAction
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
    public sealed class SigninRequestCallbackFailedAction
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