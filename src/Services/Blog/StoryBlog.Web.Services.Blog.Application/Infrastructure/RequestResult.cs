using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public struct RequestResult : IRequestResult
    {
        private IEnumerable<Exception> exceptions;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptions"></param>
        public RequestResult(IEnumerable<Exception> exceptions)
        {
            this.exceptions = exceptions ?? Enumerable.Empty<Exception>();
        }

        public static IRequestResult<TData> Success<TData>(TData data)
        {
            return new RequestResult<TData>(data);
        }

        public static IRequestResult<TData> Error<TData>(params Exception[] exceptions)
        {
            if (null == exceptions)
            {
                throw new ArgumentNullException(nameof(exceptions));
            }

            return new RequestResult<TData>(exceptions);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct RequestResult<TData> : IRequestResult<TData>
    {
        private IEnumerable<Exception> exceptions;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <summary>
        /// 
        /// </summary>
        public TData Data
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptions"></param>
        public RequestResult(IEnumerable<Exception> exceptions)
            : this(default(TData), exceptions)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public RequestResult(TData data)
            : this(data, null)
        {
        }

        private RequestResult(TData data, IEnumerable<Exception> exceptions)
        {
            this.exceptions = exceptions;
            Data = data;
        }
    }
}