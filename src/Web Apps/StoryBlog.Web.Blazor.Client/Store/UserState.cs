using Blazor.Fluxor;
using System;
using System.Security.Principal;

namespace StoryBlog.Web.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserFeature : Feature<UserState>
    {
        public override string GetName() => nameof(UserState);

        protected override UserState GetInitialState() => UserState.None;
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class UserState
    {
        public static readonly UserState None;

        public IPrincipal Principal
        {
            get;
        }

        public ModelStatus Status
        {
            get;
        }

        private UserState(ModelStatus status, IPrincipal principal)
        {
            Status = status;
            Principal = principal;
        }

        static UserState()
        {
            None = new UserState(ModelStatus.None, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static UserState Loading() => new UserState(ModelStatus.Loading, null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static UserState Failed(string error) => new UserState(ModelStatus.Failed(error), null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static UserState Success(IPrincipal principal)
        {
            if (null == principal)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            return new UserState(ModelStatus.Success, principal);
        }
    }
}