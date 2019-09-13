namespace StoryBlog.Web.Client.Store
{
    public interface IHasModelStatus
    {
        ModelStatus Status
        {
            get;
        }
    }
}