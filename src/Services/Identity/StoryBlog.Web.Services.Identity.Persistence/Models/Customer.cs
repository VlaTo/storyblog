using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StoryBlog.Web.Services.Identity.Persistence.Models
{
    public class Customer : IdentityUser<long>
    {
        [Required]
        public string ContactName
        {
            get;
            set;
        }

        public IList<Address> Addresses
        {
            get;
            internal set;
        }

        public IList<Card> Cards
        {
            get;
            internal set;
        }
    }
}