namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPagedQueryResult<out TEntity> : IQueryResult<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        NavigationCursor Backward
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        NavigationCursor Forward
        {
            get;
            set;
        }
    }
}