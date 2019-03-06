using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Identity.API.Configuration.IdentityServer;
using StoryBlog.Web.Services.Identity.API.Data.Models;
using StoryBlog.Web.Services.Identity.Domain;

namespace StoryBlog.Web.Services.Identity.API.Data
{
    public sealed class StoryBlogIdentitySetup
    {
        private readonly IHostingEnvironment environment;
        private readonly UserManager<Customer> customerManager;
        private readonly RoleManager<CustomerRole> roleManager;
        private readonly ConfigurationDbContext configurationContext;
        private readonly PersistedGrantDbContext grantContext;
        private readonly ILogger<StoryBlogIdentitySetup> logger;

        public StoryBlogIdentitySetup(
            IHostingEnvironment environment,
            UserManager<Customer> customerManager,
            RoleManager<CustomerRole> roleManager,
            ConfigurationDbContext configurationContext,
            PersistedGrantDbContext grantContext,
            ILogger<StoryBlogIdentitySetup> logger)
        {
            this.environment = environment;
            this.customerManager = customerManager;
            this.roleManager = roleManager;
            this.configurationContext = configurationContext;
            this.grantContext = grantContext;
            this.logger = logger;
        }

        public async Task SeedAsync()
        {
            if (customerManager.SupportsUserRole)
            {
                var roles = new[]
                {
                    CustomerRoleNames.Administrator,
                    CustomerRoleNames.Client,
                    CustomerRoleNames.Shopper
                };

                foreach (var roleName in roles)
                {
                    var role = await roleManager.FindByNameAsync(roleName);

                    if (null != role)
                    {
                        continue;
                    }

                    var result = await roleManager.CreateAsync(new CustomerRole
                    {
                        Name = roleName
                    });

                    if (false == result.Succeeded)
                    {
                        var descriptions = result.Errors.Select(error => error.Description);
                        throw new Exception(String.Join(';', descriptions));
                    }
                }
            }

            var users = new[]
            {
                (Email: "admin@storyblog.net", UserName: "admin", Password: "Abcd1234!", ContactName: "Administrator", Roles: new[] { CustomerRoleNames.Administrator }),
                (Email: "dev@storyblog.net", UserName: "dev", Password: "1234Abcd!", ContactName: "Developer", Roles: new[] { CustomerRoleNames.Client, CustomerRoleNames.Shopper }),
                (Email: "test@storyblog.net", UserName: "test", Password: "Test1234!", ContactName: "Test user", Roles: new[] { CustomerRoleNames.Shopper })
            };

            foreach (var user in users)
            {
                Customer customer = null;

                for (var count = 1; count >= 0; count--)
                {
                    customer = await customerManager.FindByEmailAsync(user.Email);

                    if (null != customer)
                    {
                        break;
                    }

                    var result = await customerManager.CreateAsync(new Customer
                    {
                        UserName = user.UserName,
                        ContactName = user.ContactName,
                        Email = user.Email
                    });

                    if (false == result.Succeeded)
                    {
                        break;
                    }
                }

                if (null == customer)
                {
                    logger.LogDebug("[StoryBlogIdentitySeed::SeedAsync] Failed to create customer");
                    throw new Exception();
                }
                
                if (customerManager.SupportsUserPassword && false == String.IsNullOrEmpty(user.Password))
                {
                    if (false == await customerManager.HasPasswordAsync(customer))
                    {
                        var result = await customerManager.AddPasswordAsync(customer, user.Password);

                        if (false == result.Succeeded)
                        {
                            throw new Exception();
                        }
                    }
                }

                if (customerManager.SupportsUserRole)
                {
                    var roles = await customerManager.GetRolesAsync(customer);

                    foreach (var role in user.Roles)
                    {
                        if (roles.Contains(role))
                        {
                            continue;
                        }

                        var result = await customerManager.AddToRoleAsync(customer, role);

                        if (false == result.Succeeded)
                        {
                            throw new Exception();
                        }
                    }
                }
            }

            // Identity4 seeds
            using (var scope = configurationContext.Database.BeginTransaction())
            {
                if (false == configurationContext.Clients.Any())
                {
                    foreach (var client in Config.GetClients())
                    {
                        var model = client.ToEntity();
                        configurationContext.Clients.Add(model);
                    }

                    configurationContext.SaveChanges();
                }

                if (false == configurationContext.IdentityResources.Any())
                {
                    foreach (var resource in Config.GetIdentityResources())
                    {
                        configurationContext.IdentityResources.Add(resource.ToEntity());
                    }

                    configurationContext.SaveChanges();
                }

                if (false == configurationContext.ApiResources.Any())
                {
                    foreach (var resource in Config.GetApiResources())
                    {
                        configurationContext.ApiResources.Add(resource.ToEntity());
                    }

                    configurationContext.SaveChanges();
                }

                scope.Commit();
            }
        }
    }
}