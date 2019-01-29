namespace StoryBlog.Web.Blazor.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStateMutator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mutableState"></param>
        /// <param name="mutator"></param>
        void Mutate(IMutableState mutableState, IMutator mutator);
    }
}