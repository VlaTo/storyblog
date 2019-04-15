using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Reactive
{
    internal static class TaskHelpers
    {
        private const int MAX_DELAY = int.MaxValue;

        public static Task Delay(TimeSpan delay, CancellationToken token)
        {
            var milliseconds = (long)delay.TotalMilliseconds;

            if (milliseconds > MAX_DELAY)
            {
                var remainder = delay - TimeSpan.FromMilliseconds(MAX_DELAY);

                return Task
                    .Delay(MAX_DELAY, token)
                    .ContinueWith(_ => Delay(remainder, token), TaskContinuationOptions.ExecuteSynchronously)
                    .Unwrap();
            }

            return Task.Delay(delay, token);
        }
    }
}