using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public long? ParentId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public Comment Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPublic
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CommentStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? Modified
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<Comment> Comments
        {
            get;
        }

        public Comment()
        {
            Comments = new List<Comment>();
        }
    }
}