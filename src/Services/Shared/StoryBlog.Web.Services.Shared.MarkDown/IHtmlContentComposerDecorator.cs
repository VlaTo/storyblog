using StoryBlog.Web.Services.Shared.MarkDown.Elements;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public interface IHtmlContentComposerDecorator
    {
        void Apply(HtmlTag tag, MarkDownElement element);
    }
}