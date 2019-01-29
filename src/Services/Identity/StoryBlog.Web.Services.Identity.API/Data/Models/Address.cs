using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Identity.API.Data.Models
{
    public enum AddressTypes
    {
        NotUsed = 0,
        Home,
        Billing,
        Delivery,
    }

    [Table(nameof(Address))]
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

        [Required]
        public long CustomerId
        {
            get;
            set;
        }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer
        {
            get;
            internal set;
        }
    }
}