using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninActionReducer : Reducer<UserState, SigninAction>
    {
        /// <inheritdoc cref="Reducer{TState,TAction}.Reduce" />
        public override UserState Reduce(UserState state, SigninAction action) => new UserState(ModelStatus.None);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninCallbackActionReducer : Reducer<UserState, SigninCallbackAction>
    {
        public override UserState Reduce(UserState state, SigninCallbackAction action) => new UserState(ModelStatus.Loading);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninCallbackSuccessActionReducer : Reducer<UserState, SigninCallbackSuccessAction>
    {
        public override UserState Reduce(UserState state, SigninCallbackSuccessAction action)
        {
            return new UserState(ModelStatus.Success)
            {
                Principal = action.Principal
            };
        }
    }
}