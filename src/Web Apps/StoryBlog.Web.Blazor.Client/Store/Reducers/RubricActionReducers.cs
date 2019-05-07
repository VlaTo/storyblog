using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Reducers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetRubricsActionReducer : Reducer<RubricsState, GetRubricsAction>
    {
        public override RubricsState Reduce(RubricsState state, GetRubricsAction action)
            => new RubricsState(ModelStatus.Loading, state.Rubrics);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetRubricsSuccessActionReducer : Reducer<RubricsState, GetRubricsSuccessAction>
    {
        public override RubricsState Reduce(RubricsState state, GetRubricsSuccessAction action)
            => new RubricsState(ModelStatus.Success, action.Rubrics);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetRubricsFailedActionReducer : Reducer<RubricsState, GetRubricsFailedAction>
    {
        public override RubricsState Reduce(RubricsState state, GetRubricsFailedAction action)
            => new RubricsState(ModelStatus.Failed(action.Error), state.Rubrics);
    }
}