using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryFailedAction : IAction
    {
        /// <summary>
        /// 
        /// </summary>
        public string Error
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public GetStoryFailedAction(string error)
        {
            Error = error;
        }
    }
}