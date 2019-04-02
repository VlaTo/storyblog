using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public string Token
        {
            get;
            set;
        }

        public IEnumerable<Claim> Claims
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
            Claims = Enumerable.Empty<Claim>();
        }
    }
}