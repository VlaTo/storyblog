using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Featured")]
    public class Featured
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public long StoryId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(StoryId))]
        public Story Story
        {
            get;
            set;
        }
    }
}