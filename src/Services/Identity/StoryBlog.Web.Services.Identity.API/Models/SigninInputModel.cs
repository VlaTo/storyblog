using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class SigninInputModel : CredentialsModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "RememberMeField", Prompt = "RememberMePrompt")]
        [DataMember]
        public bool RememberMe
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Url)]
        [DataMember]
        public string ReturnUrl
        {
            get;
            set;
        }
    }
}