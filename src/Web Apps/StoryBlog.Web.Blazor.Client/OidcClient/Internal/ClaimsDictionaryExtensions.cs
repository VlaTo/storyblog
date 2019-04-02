using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Internal
{
    public static class ClaimsDictionaryExtensions
    {
        public static IEnumerable<Claim> ToClaims(this IDictionary<string, object> dictionary, params string[] claimsToExclude)
        {
            var claims = new List<Claim>();
            var excludeList = claimsToExclude.ToList();

            foreach (var kvp in dictionary)
            {
                if (excludeList.Contains(kvp.Key))
                {
                    continue;
                }

                /*if (kvp.Value is IEnumerable<> array)
                {
                    foreach (var item in array)
                    {
                        claims.Add(new Claim(x.Key, item.ToString()));
                    }
                }
                else
                {*/
                claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                //}
            }

            return claims;
        }
    }
}