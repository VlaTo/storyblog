using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Authors")]
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        [Required]
        public string UserName
        {
            get;
            set;
        }

        public IList<Address> Addresses
        {
            get;
            internal set;
        }

        public IList<Story> Stories
        {
            get;
            internal set;
        }

        public Author()
        {
            Addresses = new List<Address>();
            Stories = new List<Story>();
        }
    }
}