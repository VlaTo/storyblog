using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UserActionReducers : Reducer<UserState, LoginAction>
    {
        /// <inheritdoc cref="Reducer{TState,TAction}.Reduce" />
        public override UserState Reduce(UserState state, LoginAction action) => new UserState(ModelStatus.Loading);
    }
}