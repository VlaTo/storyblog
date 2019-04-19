using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Services.Identity.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Identity.Persistence.Models;

namespace StoryBlog.Web.Services.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityFrameworkProfileService : IProfileService
    {
        private readonly UserManager<Customer> userManager;
        private readonly RoleManager<CustomerRole> roleManager;
        private readonly IUserClaimsPrincipalFactory<Customer> claimsFactory;
        private readonly IdentityOptions identityOptions;

        public EntityFrameworkProfileService(
            UserManager<Customer> userManager,
            RoleManager<CustomerRole> roleManager,
            IUserClaimsPrincipalFactory<Customer> claimsFactory,
            IOptions<IdentityOptions> identityOptionsAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.claimsFactory = claimsFactory;
            identityOptions = identityOptionsAccessor.Value;
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

            var subjectId = context.Subject.GetSubjectId();

            if (null == subjectId)
            {
                throw new ArgumentException();
            }

            var user = await userManager.FindByIdAsync(subjectId);

            if (null == user)
            {
                throw new ArgumentNullException();
            }

            //var temp = await claimsFactory.CreateAsync(user);
            var claims = await GenerateClaimsAsync(user, context.IssuedClaims, context.RequestedClaimTypes);

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

        private async Task<IEnumerable<Claim>> GenerateClaimsAsync(
            Customer user,
            IList<Claim> issuedClaims,
            IEnumerable<string> requestedClaimTypes)
        {
            var id = await userManager.GetUserIdAsync(user);
            var name = await userManager.GetUserNameAsync(user);
            var claims = new List<Claim>
            {
                new Claim(identityOptions.ClaimsIdentity.UserIdClaimType,id),
                new Claim(identityOptions.ClaimsIdentity.UserNameClaimType,name),
            };

            //AddCustomRequestedClaim(user, requestedClaimTypes, claims);

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

            if (userManager.SupportsUserPhoneNumber && false == String.IsNullOrEmpty(user.PhoneNumber))
            {
                claims.AddRange(new[]
                {
                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                    new Claim(JwtClaimTypes.PhoneNumberVerified,
                        user.PhoneNumberConfirmed ? bool.TrueString : bool.FalseString,
                        ClaimValueTypes.Boolean)
                });
            }

            if (userManager.SupportsUserRole)
            {
                var claimType = identityOptions.ClaimsIdentity.RoleClaimType;

                if (requestedClaimTypes.Contains(claimType))
                {
                    var roles = await userManager.GetRolesAsync(user);

                    foreach (var roleName in roles)
                    {
                        claims.Add(new Claim(claimType, roleName));

                        if (false == roleManager.SupportsRoleClaims)
                        {
                            continue;
                        }

                        var role = await roleManager.FindByNameAsync(roleName);

                        if (null != role)
                        {
                            claims.AddRange(await roleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }

            if (userManager.SupportsUserSecurityStamp)
            {
                var stamp = await userManager.GetSecurityStampAsync(user);
                claims.Add(new Claim(identityOptions.ClaimsIdentity.SecurityStampClaimType, stamp));
            }

            if (userManager.SupportsUserClaim)
            {
                var userClaims = await userManager.GetClaimsAsync(user);
                claims.AddRange(userClaims);
            }

            foreach (var issuedClaim in issuedClaims)
            {
                var claim = GetIssuedClaim(issuedClaim);

                if (null == claim)
                {
                    continue;
                }

                claims.Add(claim);
            }

            return claims;
        }

        /*private static void AddCustomRequestedClaim(IList<Claim> claims, string claimType, string claimValue, IEnumerable<string> requestedClaimTypes)
        {
            if (String.IsNullOrWhiteSpace(claimValue))
            {
                return;
            }

            if (requestedClaimTypes.Has(claimType))
            {
                claims.Add(new Claim(claimType, claimValue));
            }
        }*/

        private static bool IsUserActive(Customer user)
        {
            return false == user.LockoutEnabled ||
                   false == user.LockoutEnd.HasValue ||
                   user.LockoutEnd <= DateTime.UtcNow;
        }

        private static Claim GetIssuedClaim(Claim issuedClaim)
        {
            return null;
        }
    }
}