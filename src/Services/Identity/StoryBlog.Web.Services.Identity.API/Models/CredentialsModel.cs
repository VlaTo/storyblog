using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Identity.API.Models
{
    [DataContract]
    public class CredentialsModel
    {
        [Display(Name = "EmailField", Prompt = "EmailPrompt")]
        [Required(ErrorMessage = "EmailRequiredError")]
        [DataType(DataType.EmailAddress, ErrorMessage = "EmailInvalidError")]
        [DataMember]
        public string Email
        {
            get;
            set;
        }

        [Display(Name = "PasswordField", Prompt = "PasswordPrompt")]
        [Required(ErrorMessage = "PasswordRequiredError")]
        [DataType(DataType.Password, ErrorMessage = "PasswordInvalidError")]
        [DataMember]
        public string Password
        {
            get;
            set;
        }
    }
}