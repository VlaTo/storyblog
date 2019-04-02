using System.Diagnostics;
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
            Debug.WriteLine($"[UserActionReducers.Reduce] token: {action.Token}");
            return new UserState(ModelStatus.Success)
            {
                Token = action.Token
            };
        }
    }
}