using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserFeature : Feature<UserState>
    {
        public override string GetName() => nameof(UserState);

        protected override UserState GetInitialState()
        {
            return new UserState(ModelStatus.None);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class UserState
    {
        public IPrincipal Principal
        {
            get;
            set;
        }

        public ModelStatus Status
        {
            get;
        }

        public UserState(ModelStatus status)
        {
            Status = status;
        }
    }
}