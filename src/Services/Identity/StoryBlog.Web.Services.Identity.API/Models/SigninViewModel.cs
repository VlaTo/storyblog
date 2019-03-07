using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    public sealed class SigninViewModel : SigninModel
    {
        public bool AllowRememberMe
        {
            get;
            set;
        }

        public bool EnableLocalLogin
        {
            get;
            set;
        }

        public ICollection<ExternalProvider> ExternalProviders
        {
            get;
            set;
        }

        public bool IsExternalLoginOnly => false == EnableLocalLogin && ExternalProviders.Any();

        public IEnumerable<ExternalProvider> VisibleProviders =>
            ExternalProviders.Where(provider => false == String.IsNullOrEmpty(provider.DisplayName));

        public string ExternalAuthenticationScheme =>
            IsExternalLoginOnly ? ExternalProviders.FirstOrDefault()?.AuthenticationScheme : null;
    }
}