using MediatR;
using StoryBlog.Web.Services.Identity.Application.Services;
using StoryBlog.Web.Services.Identity.Application.Signin.Models;
using StoryBlog.Web.Services.Identity.Application.Signin.Queries;
using StoryBlog.Web.Services.Identity.Persistence.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Signin.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public sealed class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerResult>
    {
        private readonly ILoginService<Customer> loginService;

        public GetCustomerQueryHandler(ILoginService<Customer> loginService)
        {
            this.loginService = loginService;
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public async Task<CustomerResult> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await loginService.FindByEmailAsync(request.Email);

            if (null == customer)
            {
                throw new InvalidOperationException();
            }

            var signin = await loginService.ValidateCredentialsAsync(customer, request.Password);

            if (signin.Succeeded)
            {
                return CustomerResult.Succeeded(customer);
            }

            if (signin.IsNotAllowed)
            {
                return CustomerResult.NotAllowed(customer);
            }

            if (signin.IsLockedOut)
            {
                return CustomerResult.LockedOut(customer);
            }

            if (signin.RequiresTwoFactor)
            {
                return CustomerResult.TwoFactorRequired(customer);
            }

            return CustomerResult.Failed();
        }
    }
}