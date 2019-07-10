using System;

namespace StoryBlog.Web.Services.Identity.Application.Configuration
{
    public static class AccountOptions
    {
        public static readonly bool AllowLocalLogin = true;

        public static readonly bool AllowRememberMe = true;

        public static readonly string WindowsAuthenticationScheme = "Windows"; // IISDefaults.AuthenticationScheme;

        public static readonly TimeSpan RememberMeSigninDuration = TimeSpan.FromDays(30.0d);

        public static readonly bool IncludeWindowsGroups = true;
    }
}