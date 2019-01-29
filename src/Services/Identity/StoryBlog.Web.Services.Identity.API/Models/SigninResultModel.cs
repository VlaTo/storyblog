using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    [DataContract(Name = "result")]
    public class SigninResultModel
    {
        [DataMember(Name = "email")]
        public string Email
        {
            get;
            set;
        }

        [DataMember(Name = "username")]
        public string UserName
        {
            get;
            set;
        }
    }
}