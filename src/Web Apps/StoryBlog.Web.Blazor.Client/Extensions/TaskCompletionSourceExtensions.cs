using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Extensions
{
    internal static class TaskCompletionSourceExtensions
    {
        public static async Task<T> AsTask<T>(this TaskCompletionSource<T> completion, CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(() => completion.TrySetCanceled(cancellationToken)))
            {
                return await completion.Task;
            }
        }
    }
}