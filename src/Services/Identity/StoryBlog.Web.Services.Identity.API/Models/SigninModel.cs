using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    [DataContract]
    public class SigninModel : CredentialsModel
    {
        [Display(Name = "RememberMeField", Prompt = "RememberMePrompt")]
        [DataMember]
        public bool RememberMe
        {
            get;
            set;
        }

        [DataType(DataType.Url)]
        [DataMember]
        public string ReturnUrl
        {
            get;
            set;
        }
    }
}