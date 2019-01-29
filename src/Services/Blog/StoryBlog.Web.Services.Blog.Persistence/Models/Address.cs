using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Blog.Persistence.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum AddressTypes
    {
        NotUsed = 0,
        Home,
        Billing,
        Delivery,
    }

    /// <summary>
    /// 
    /// </summary>
    [Table("Addresses")]
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        [Required]
        public string City
        {
            get;
            set;
        }

        public string Street
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        [Required]
        public string Country
        {
            get;
            set;
        }

        [Required]
        [DefaultValue(AddressTypes.NotUsed)]
        public AddressTypes AddressTypes
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
            internal set;
        }
    }
}