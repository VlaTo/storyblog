using MediatR;
using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Services.Identity.Application.Services;
using StoryBlog.Web.Services.Identity.Application.Signin.Models;
using StoryBlog.Web.Services.Identity.Application.Signin.Queries;
using StoryBlog.Web.Services.Identity.Persistence.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Signin.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, IRequestResult<CustomerResult>>
    {
        private readonly ILoginService<Customer> loginService;

        public GetCustomerQueryHandler(ILoginService<Customer> loginService)
        {
            this.loginService = loginService;
        }

        public async Task<IRequestResult<CustomerResult>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await loginService.FindByEmailAsync(request.Email);

            if (null == customer)
            {
                return RequestResult.Error<CustomerResult>(new InvalidOperationException());
            }

            var signin = await loginService.ValidateCredentialsAsync(customer, request.Password);

            if (SignInResult.Failed == signin)
            {
                return RequestResult.Error<CustomerResult>();
            }

            CustomerResult result = null;

            if (signin.Succeeded)
            {
                result = CustomerResult.Succeeded(customer);
            }

            if (signin.IsNotAllowed)
            {
                result = CustomerResult.NotAllowed(customer);
            }

            if (signin.IsLockedOut)
            {
                result = CustomerResult.LockedOut(customer);
            }

            if (signin.RequiresTwoFactor)
            {
                result = CustomerResult.TwoFactorRequired(customer);
            }

            return RequestResult.Success(result);
        }
    }
}