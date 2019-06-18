using MediatR;
using StoryBlog.Web.Services.Identity.Application.Signin.Models;

namespace StoryBlog.Web.Services.Identity.Application.Signin.Queries
{
    public sealed class GetCustomerQuery : IRequest<CustomerResult>
    {
        public string Email
        {
            get;
        }

        public string Password
        {
            get;
        }

        public GetCustomerQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}