namespace StoryBlog.Web.Services.Shared.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVisitor<in T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void Visit(T obj);
    }
}