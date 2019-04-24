using MediatR;
using StoryBlog.Web.Services.Identity.Persistence.Models;

namespace StoryBlog.Web.Services.Identity.Application.Signin.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class SigninCommand : IRequest
    {
        public bool ShouldPersist
        {
            get;
        }

        public Customer Customer
        {
            get;
        }

        public SigninCommand(Customer customer, bool shouldPersist = true)
        {
            Customer = customer;
            ShouldPersist = shouldPersist;
        }
    }
}