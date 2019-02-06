using System;
using System.Collections.Generic;
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

    /// <summary>
    /// 
    /// </summary>
    [Table("Stories")]
    public class Story
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

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.Text)]
        public string Slug
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long AuthorId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(AuthorId))]
        public Author Author
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public StoryStatus Status
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
        public bool IsPublic
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

        /// <summary>
        /// 
        /// </summary>
        public Story()
        {
            Comments = new List<Comment>();
        }
    }
}