using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResources"></typeparam>
    public interface IPagedQueryResult<out TEntity, out TResources> : IQueryResult<TEntity, TResources>
        where TResources : IQueryResultResources
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