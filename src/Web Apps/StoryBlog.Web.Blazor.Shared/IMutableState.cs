namespace StoryBlog.Web.Blazor.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMutableState
    {
        /// <summary>
        /// 
        /// </summary>
        int Version
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        void Apply(IMutator action);
    }
}