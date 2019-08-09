namespace StoryBlog.Web.Services.Shared.Common
{
    public interface IPluralLocalizer
    {
        string this[string noun, int numerator]
        {
            get;
        }
    }
}