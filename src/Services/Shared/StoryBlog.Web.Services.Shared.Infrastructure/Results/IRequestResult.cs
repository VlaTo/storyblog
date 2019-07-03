namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestResult
    {
        bool IsSucceeded
        {
            get;
        }

        bool IsFailed
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRequestResult<out TEntity> : IRequestResult
    {
        /// <summary>
        /// 
        /// </summary>
        TEntity Entity
        {
            get;
        }
    }
}