using System.Threading;

namespace StoryBlog.Web.Services.Shared.Common.Synchronization.Infrastructure
{
    public sealed class AggregatedCancellationToken
    {
        private readonly CancellationTokenSource cts;

        /// <summary>
        /// 
        /// </summary>
        public CancellationToken Token
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public AggregatedCancellationToken()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cts"></param>
        public AggregatedCancellationToken(CancellationTokenSource cts)
        {
            this.cts = cts;
            Token = cts.Token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        public AggregatedCancellationToken(CancellationToken ct)
        {
            Token = ct;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            cts.Dispose();
        }
    }
}