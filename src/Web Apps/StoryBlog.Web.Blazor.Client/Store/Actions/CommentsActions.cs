namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateNewCommentActions
    {
        public string Message
        {
            get;
        }

        public CreateNewCommentActions(string message)
        {
            Message = message;
        }
    }
}