using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RequestResult
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Exception> Exceptions
        {
            get;
        }

        protected RequestResult(IEnumerable<Exception> exceptions)
        {
            Exceptions = exceptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public static RequestResult Error(params Exception[] exceptions)
        {
            if (null == exceptions)
            {
                throw new ArgumentNullException(nameof(exceptions));
            }

            return new InternalRequestResult(exceptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RequestResult Success()
        {
            return new InternalRequestResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static RequestResult<TResult> Success<TResult>(TResult result)
        {
            return new RequestResult<TResult>(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static QueryResult<TEntity> Success<TEntity>(IEnumerable<TEntity> result)
        {
            var list = new List<TEntity>(result);
            return new QueryResult<TEntity>(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static QueryResult<TEntity> Success<TEntity>(IList<TEntity> list)
        {
            var collection = new ReadOnlyCollection<TEntity>(list);
            return new QueryResult<TEntity>(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static QueryResult<TEntity> Success<TEntity>(IReadOnlyCollection<TEntity> collection)
        {
            return new QueryResult<TEntity>(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class InternalRequestResult : RequestResult
        {
            /// <summary>
            /// 
            /// </summary>
            internal InternalRequestResult()
                : this(Enumerable.Empty<Exception>())
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="exceptions"></param>
            internal InternalRequestResult(IEnumerable<Exception> exceptions)
                : base(exceptions)
            {
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class RequestResult<TData> : RequestResult
    {
        /// <summary>
        /// 
        /// </summary>
        public TData Data
        {
            get;
        }

        internal RequestResult(TData data)
            : base(Enumerable.Empty<Exception>())
        {
            Data = data;
        }
    }
}