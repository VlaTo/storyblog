using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

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
        public DateTime? Modified
        {
            get;
            set;
        }

        public bool IsPublic
        {
            get;
            set;
        }

        public IList<Comment> Comments
        {
            get;
            set;
        }

        public Story()
        {
            //Comments = new List<Comment>();
        }
    }
}