using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface INavigateableQueryResult<out TEntity> : IQueryResult<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        NavigationCursor Backward
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        NavigationCursor Forward
        {
            get;
        }
    }
}