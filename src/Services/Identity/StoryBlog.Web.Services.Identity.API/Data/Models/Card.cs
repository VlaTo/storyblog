using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoryBlog.Web.Services.Identity.API.Data.Models
{
    [Table(nameof(Card))]
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id
        {
            get;
            set;
        }

        [Required]
        [StringLength(20, MinimumLength = 16)]
        public string Number
        {
            get;
            set;
        }

        [Required]
        [StringLength(3)]
        public string Code
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Expiration
        {
            get;
            set;
        }

        public long CustomerId
        {
            get;
            set;
        }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer
        {
            get;
            set;
        }
    }
}