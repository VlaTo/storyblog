using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using StoryBlog.Web.Services.Identity.Application.Configuration;
using StoryBlog.Web.Services.Identity.Application.Services;
using StoryBlog.Web.Services.Identity.Application.Signin.Commands;
using StoryBlog.Web.Services.Identity.Persistence.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Identity.Application.Signin.Handlers
{
    // ReSharper disable once UnusedMember.Global
    public class SignInCommandHandler : IRequestHandler<SignInCommand>
    {
        private readonly ILoginService<Customer> loginService;

        public SignInCommandHandler(ILoginService<Customer> loginService)
        {
            this.loginService = loginService;
        }

        public async Task<Unit> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            AuthenticationProperties properties = null;

            if (AccountOptions.AllowRememberMe && request.ShouldPersist)
            {
                properties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow + AccountOptions.RememberMeSigninDuration
                };
            }

            await loginService.SigninAsync(
                request.Customer,
                properties,
                IdentityServerConstants.LocalIdentityProvider
            );

            return Unit.Value;
        }
    }
}