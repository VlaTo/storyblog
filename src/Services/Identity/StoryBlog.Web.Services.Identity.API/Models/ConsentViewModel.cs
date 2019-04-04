using System.Collections.Generic;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConsentViewModel : ConsentInputModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ClientName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClientUrl
        {
            get;
            set;
        }

        public string ClientLogoUrl
        {
            get;
            set;
        }

        public bool AllowRememberConsent
        {
            get;
            set;
        }

        public IEnumerable<ScopeViewModel> IdentityScopes
        {
            get;
            set;
        }

        public IEnumerable<ScopeViewModel> ResourceScopes
        {
            get;
            set;
        }
    }
}