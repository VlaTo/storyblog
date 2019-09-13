using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Extensions
{
    internal static class ValueTaskExtensions
    {
        public static void RunAndForget(this ValueTask task)
        {
            if (null == task)
            {
                throw new ArgumentNullException(nameof(task));
            }

            // do nothing with task
        }
    }
}