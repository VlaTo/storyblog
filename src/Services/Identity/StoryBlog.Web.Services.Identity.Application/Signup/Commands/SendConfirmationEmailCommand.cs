using MediatR;
using StoryBlog.Web.Services.Identity.Persistence.Models;

namespace StoryBlog.Web.Services.Identity.Application.Signup.Commands
{
    public sealed class SendConfirmationEmailCommand : IRequest
    {
        public Customer Customer
        {
            get;
        }

        public SendConfirmationEmailCommand(Customer customer)
        {
            Customer = customer;
        }
    }
}