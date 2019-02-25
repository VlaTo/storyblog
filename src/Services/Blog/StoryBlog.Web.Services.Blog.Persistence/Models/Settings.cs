using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    [Table("Settings")]
    public class Settings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        [Required]
        [DataMember]
        [DataType(DataType.Text)]
        public string Name
        {
            get;
            set;
        }

        [Required]
        [DataMember]
        [DataType(DataType.Custom)]
        public byte[] Value
        {
            get;
            set;
        }
    }
}