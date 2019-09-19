namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public interface IVisitor<in T>
    {
        void Visit(T obj);
    }
}