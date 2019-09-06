namespace StoryBlog.Web.Services.Shared.Common
{
    public interface IPluralService
    {
        string this[string noun, long numerator]
        {
            get;
        }
    }
}