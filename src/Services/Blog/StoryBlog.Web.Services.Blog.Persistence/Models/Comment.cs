using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum CommentStatus
    {
        /// <summary>
        /// 
        /// </summary>
        Draft,

        /// <summary>
        /// 
        /// </summary>
        Review,

        /// <summary>
        /// 
        /// </summary>
        Published
    }

    /// <summary>
    /// 
    /// </summary>
    [Table("Comments")]
    public class Comment
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

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content
        {
            get;
            set;
        }

        public long AuthorId
        {
            get;
            set;
        }

        [ForeignKey(nameof(AuthorId))]
        public Author Author
        {
            get;
            set;
        }

        public long StoryId
        {
            get;
            set;
        }

        [ForeignKey(nameof(StoryId))]
        public Story Story
        {
            get;
            set;
        }

        public bool IsPublic
        {
            get;
            set;
        }

        public CommentStatus Status
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
        public DateTime? Modified
        {
            get;
            set;
        }
    }
}