using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Extensions
{
    internal static class TaskExtensions
    {
        public static void RunAndForget(this Task task)
        {
            if (null == task)
            {
                throw new ArgumentNullException(nameof(task));
            }

            // do nothing with task
        }

        public static void RunAndForget<T>(this Task<T> task)
        {
            if (null == task)
            {
                throw new ArgumentNullException(nameof(task));
            }

            // do nothing with task
        }
    }
}