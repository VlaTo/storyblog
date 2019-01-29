using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Services.Identity.API.Data.Models;
using StoryBlog.Web.Services.Identity.Domain;

namespace StoryBlog.Web.Services.Identity.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityFrameworkProfileService : IProfileService
    {
        private readonly UserManager<Customer> userManager;

        public EntityFrameworkProfileService(UserManager<Customer> userManager)
        {
            this.userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (null == context.Subject)
            {
                throw new ArgumentNullException(nameof(context.Subject));
            }

            var subject = context.Subject;
            var subjectId = subject.Claims.First(claim => claim.Type == JwtClaimTypes.Subject);

            if (null == subjectId)
            {
                throw new ArgumentException();
            }

            var user = await userManager.FindByIdAsync(subjectId.Value);

            if (null == user)
            {
                throw new ArgumentNullException();
            }

            var claims = GetClaimsFromUser(user);

            context.IssuedClaims = claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (null == context.Subject)
            {
                throw new ArgumentNullException(nameof(context.Subject));
            }

            var subject = context.Subject;
            var subjectId = subject.Claims.First(claim => claim.Type == JwtClaimTypes.Subject);

            if (null == subjectId)
            {
                throw new ArgumentException();
            }

            var user = await userManager.FindByIdAsync(subjectId.Value);

            context.IsActive = false;

            if (null != user)
            {
                if (userManager.SupportsUserSecurityStamp)
                {
                    var securityStamp = subject.Claims
                        .Where(claim => claim.Type == "security_stamp")
                        .Select(claim => claim.Value)
                        .SingleOrDefault();

                    if (null != securityStamp)
                    {
                        var temp = await userManager.GetSecurityStampAsync(user);

                        if (securityStamp != temp)
                        {
                            return;
                        }
                    }
                }

                context.IsActive = IsUserActive(user);
            }
        }

        private IEnumerable<Claim> GetClaimsFromUser(Customer user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            if (String.IsNullOrWhiteSpace(user.ContactName))
            {
                claims.Add(new Claim(CustomerClaimTypes.Name, user.NormalizedUserName));
            }

            var address = user.Addresses?.FirstOrDefault(x => x.AddressTypes == AddressTypes.Home);

            if (null != address)
            {
                claims.AddRange(new[]
                {
                    new Claim(AddressClaimTypes.City, address.City),
                    new Claim(AddressClaimTypes.Street, address.Street)
                });
            }

            if (userManager.SupportsUserEmail)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.EmailVerified,
                        user.EmailConfirmed ? bool.TrueString : bool.FalseString,
                        ClaimValueTypes.Boolean)
                });
            }

            if (userManager.SupportsUserPhoneNumber)
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified,
                        user.PhoneNumberConfirmed ? bool.TrueString : bool.FalseString,
                        ClaimValueTypes.Boolean)
                });
            }

            return claims;
        }

        private static bool IsUserActive(Customer user)
        {
            return false == user.LockoutEnabled ||
                   false == user.LockoutEnd.HasValue ||
                   user.LockoutEnd <= DateTime.UtcNow;
        }
    }
}