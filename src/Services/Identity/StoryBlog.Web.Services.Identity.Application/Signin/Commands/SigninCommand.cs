using MediatR;
using StoryBlog.Web.Services.Identity.Persistence.Models;

namespace StoryBlog.Web.Services.Identity.Application.Signin.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class SignInCommand : IRequest
    {
        public bool ShouldPersist
        {
            get;
        }

        public Customer Customer
        {
            get;
        }

        public SignInCommand(Customer customer, bool shouldPersist = true)
        {
            Customer = customer;
            ShouldPersist = shouldPersist;
        }
    }
}