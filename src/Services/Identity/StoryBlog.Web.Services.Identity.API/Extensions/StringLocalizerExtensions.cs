using System;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace StoryBlog.Web.Services.Identity.API.Extensions
{
    internal static class StringLocalizerExtensions
    {
        public static string InvalidCredentials(this IStringLocalizer localizer)
        {
            if (null == localizer)
            {
                throw new ArgumentNullException(nameof(localizer));
            }

            var result = localizer.WithCulture(CultureInfo.CurrentUICulture)["InvalidCredentials"];

            return result.ResourceNotFound ? result.Name : result.Value;
        }
    }
}