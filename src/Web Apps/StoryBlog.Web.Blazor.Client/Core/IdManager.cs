using System.Collections.Generic;
using System.Globalization;

namespace StoryBlog.Web.Blazor.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IdManager : IIdManager
    {
        private readonly IDictionary<string, long> ids;

        /// <summary>
        /// 
        /// </summary>
        public IdManager()
        {
            ids = new Dictionary<string, long>();
        }

        /// <inheritdoc cref="IIdManager.GenerateId" />
        public string GenerateId(string prefix)
        {
            if (false == ids.TryGetValue(prefix, out var lastId))
            {
                lastId = 0L;
                ids.Add(prefix, lastId);
            }
            else
            {
                ids[prefix] = (lastId + 1);
            }

            return prefix + '-' + lastId.ToString("D", CultureInfo.InvariantCulture);
        }
    }
}