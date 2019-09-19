using StoryBlog.Web.Services.Shared.MarkDown.Elements;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public interface IMarkDownDecorator<in TElement>
        where TElement : MarkDownElement
    {
        void Apply(TElement element);
    }
}