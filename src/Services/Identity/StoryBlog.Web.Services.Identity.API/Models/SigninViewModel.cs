using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    [DataContract]
    public class SigninViewModel : SigninModel
    {
        [DataMember]
        public bool AllowRememberMe
        {
            get;
            set;
        }

        [DataMember]
        public bool EnableLocalLogin
        {
            get;
            set;
        }

        [DataMember]
        public ICollection<ExternalProvider> ExternalProviders
        {
            get;
            set;
        }

        [IgnoreDataMember]
        public string CaptchaKey
        {
            get;
            set;
        }

        [DataMember]
        public bool IsExternalLoginOnly => false == EnableLocalLogin && ExternalProviders.Any();

        [DataMember]
        public IEnumerable<ExternalProvider> VisibleProviders =>
            ExternalProviders.Where(provider => false == String.IsNullOrEmpty(provider.DisplayName));

        [DataMember]
        public string ExternalAuthenticationScheme =>
            IsExternalLoginOnly ? ExternalProviders.FirstOrDefault()?.AuthenticationScheme : null;
    }
}