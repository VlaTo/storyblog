using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    public enum CommentStatus
    {
        Draft,
        Review,
        Published
    }

    [Table("Comments")]
    public class Comment
    {
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