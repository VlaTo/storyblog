using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Rubrics")]
    public class Rubric
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        [DefaultValue(-1)]
        [DataType(DataType.Custom)]
        public int Order
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Text)]
        public string Name
        {
            get;
            set;
        }

        [DataType(DataType.Text)]
        public string Slug
        {
            get;
            set;
        }

        public IList<Story> Stories
        {
            get;
        }

        public Rubric()
        {
            Stories = new List<Story>();
        }
    }
}