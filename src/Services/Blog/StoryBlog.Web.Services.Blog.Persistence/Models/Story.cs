using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum StoryStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Draft,

        /// <summary>
        /// 
        /// </summary>
        Published
    }

    [Table("Stories")]
    public class Story
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        [Required]
        public string Title
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

        [DataType(DataType.MultilineText)]
        public string Content
        {
            get;
            set;
        }

        [ForeignKey(nameof(Author))]
        public long AuthorId
        {
            get;
            set;
        }

        public Author Author
        {
            get;
            set;
        }

        public StoryStatus Status
        {
            get;
            set;
        }

        [DataType(DataType.DateTime)]
        public DateTime Created
        {
            get;
            set;
        }

        [DataType(DataType.DateTime)]
        public DateTime Modified
        {
            get;
            set;
        }

        public bool IsPublic
        {
            get;
            set;
        }
    }
}