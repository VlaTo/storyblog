using System;
using Microsoft.Extensions.Localization;

namespace StoryBlog.Web.Services.Identity.API.Extensions
{
    internal static class StringLocalizerExtensions
    {
        public static string InvalidCredentials(this IStringLocalizer localizer, string uiLocales)
        {
            if (null == localizer)
            {
                throw new ArgumentNullException(nameof(localizer));
            }

            var result = localizer["InvalidCredentials"];

            return result.ResourceNotFound ? result.Name : result.Value;
        }
    }
}