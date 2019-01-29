using System.Threading;

namespace StoryBlog.Web.Services.Shared.Common.Synchronization.Infrastructure
{
    // ReSharper disable once UnusedTypeParameter
    internal static class IdManager<TTag>
    {
        private static int lastId;

        public static int GetId(ref int id)
        {
            if (0 != id)
            {
                return id;
            }

            int candidate;

            do
            {
                candidate = Interlocked.Increment(ref lastId);

            } while (0 == candidate);

            Interlocked.CompareExchange(ref id, candidate, 0);

            return id;
        }
    }
}