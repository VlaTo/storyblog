using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Identity.API.Extensions
{
    internal static class ClaimTypesListExtensions
    {
        public static bool Has(this IEnumerable<string> claimTypes, string claim)
        {
            if (null == claimTypes)
            {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            if (String.IsNullOrEmpty(claim))
            {
                return false;
            }

            var comparer = StringComparer.InvariantCulture;

            return claimTypes.Any(current => comparer.Equals(current, claim));
        }
    }
}
