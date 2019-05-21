using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    /// <summary>
    /// The status of the story.
    /// </summary>
    public enum StoryStatus
    {
        /// <summary>
        /// The story is draft.
        /// </summary>
        Draft,

        /// <summary>
        /// The story is published.
        /// </summary>
        Published
    }

    /// <summary>
    /// The <see cref="Story" /> entity definition.
    /// Describes stored story representation and fields definition.
    /// </summary>
    [Table("Stories")]
    public class Story
    {
        /// <summary>
        /// Gets or sets the identifier of the <see cref="Story" /> object.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Title"/> of the <see cref="Story" /> object.
        /// </summary>
        [Required]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Slug"/> of the <see cref="Story" /> object.
        /// </summary>
        [DataType(DataType.Text)]
        public string Slug
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Content"/> of the <see cref="Story" /> object.
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="AuthorId"/> of the <see cref="Story" /> object.
        /// </summary>
        public long AuthorId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Author"/> of the <see cref="Story" /> object.
        /// </summary>
        [ForeignKey(nameof(AuthorId))]
        public Author Author
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Status"/> of the <see cref="Story" /> object.
        /// </summary>
        public StoryStatus Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Created"/> date and time of the <see cref="Story" /> object.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTimeOffset Created
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Modified"/> date and time of the <see cref="Story" /> object.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTimeOffset? Modified
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Published"/> date and time of the <see cref="Story" /> object.
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTimeOffset? Published
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="IsPublic"/> of the <see cref="Story" /> object.
        /// </summary>
        public bool IsPublic
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the <see cref="Comments"/> of the <see cref="Story" /> object.
        /// </summary>
        public IList<Comment> Comments
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Rubrics"/> collection of the related <see cref="Rubric" /> objects.
        /// </summary>
        public IList<Rubric> Rubrics
        {
            get;
        }

        /// <summary>
        /// Instantiates new instance of the <see cref="Story" /> object.
        /// </summary>
        public Story()
        {
            Comments = new List<Comment>();
            Rubrics = new List<Rubric>();
        }
    }
}