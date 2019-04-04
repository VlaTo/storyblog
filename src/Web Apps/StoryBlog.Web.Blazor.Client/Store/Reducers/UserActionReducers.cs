using System.Linq;
using System.Security.Claims;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginUserActionReducer : Reducer<UserState, LoginAction>
    {
        /// <inheritdoc cref="Reducer{TState,TAction}.Reduce" />
        public override UserState Reduce(UserState state, LoginAction action) => new UserState(ModelStatus.Loading);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginSuccessActionReducer : Reducer<UserState, LoginSuccessAction>
    {
        public override UserState Reduce(UserState state, LoginSuccessAction action)
        {
            return new UserState(ModelStatus.Success)
            {
                Token = action.Token,
                Claims = action.Claims
            };
        }
    }

    public sealed class GetUserInfoActionReducer : Reducer<UserState, GetUserInfoAction>
    {
        public override UserState Reduce(UserState state, GetUserInfoAction action)
        {
            return new UserState(ModelStatus.Loading)
            {
                Token = action.Token,
                Claims = Enumerable.Empty<Claim>()
            };
        }
    }
}