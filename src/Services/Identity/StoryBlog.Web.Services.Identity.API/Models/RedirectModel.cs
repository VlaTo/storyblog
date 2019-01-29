using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    [DataContract]
    public class RedirectModel
    {
        [DataMember]
        public string RedirectUrl
        {
            get;
            set;
        }
    }
}