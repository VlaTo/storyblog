namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestResult : IRequestResult
    {
        public bool IsSucceeded { get; }

        public bool IsFailed { get; }

        protected RequestResult(bool isSucceeded = true, bool isFailed = false)
        {
            IsSucceeded = isSucceeded;
            IsFailed = isFailed;
        }

        public static IRequestResult Success()
        {
            return new RequestResult();
        }

        public static IRequestResult<TEntity> Success<TEntity>(TEntity data)
        {
            return new EntityRequestResult<TEntity>(data);
        }

        public static IRequestResult<TEntity> Failed<TEntity>()
        {
            return new EntityRequestResult<TEntity>(isFailed: true);
        }

        public static IRequestResult Failed()
        {
            return new RequestResult(isFailed: true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        private class EntityRequestResult<TEntity> : RequestResult, IRequestResult<TEntity>
        {
            /// <summary>
            /// 
            /// </summary>
            public TEntity Entity { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="entity"></param>
            internal EntityRequestResult(TEntity entity)
            {
                Entity = entity;
            }

            internal EntityRequestResult(bool isSucceeded = true, bool isFailed = false)
                : base(isSucceeded, isFailed)
            {
            }
        }
    }
}