using Blazor.Fluxor;
using StoryBlog.Web.Client.Store.Actions;

namespace StoryBlog.Web.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestActionReducer : Reducer<UserState, SigninRequestAction>
    {
        /// <inheritdoc cref="Reducer{TState,TAction}.Reduce" />
        public override UserState Reduce(UserState state, SigninRequestAction action) => UserState.None;
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestFailedActionReducer : Reducer<UserState, SigninRequestFailedAction>
    {
        public override UserState Reduce(UserState state, SigninRequestFailedAction action) =>
            UserState.Failed(action.Error);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestCallbackSuccessActionReducer : Reducer<UserState, SigninRequestSuccessAction>
    {
        public override UserState Reduce(UserState state, SigninRequestSuccessAction action) =>
            UserState.Success(action.Principal);
    }
}