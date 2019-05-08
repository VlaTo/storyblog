namespace StoryBlog.Web.Blazor.Client.Store
{
    public interface IHasModelStatus
    {
        ModelStatus Status
        {
            get;
        }
    }
}