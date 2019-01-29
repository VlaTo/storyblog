using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    [DataContract]
    public class ExternalProvider
    {
        [DataMember]
        public string DisplayName
        {
            get;
            set;
        }

        [DataMember]
        public string AuthenticationScheme
        {
            get;
            set;
        }
    }
}