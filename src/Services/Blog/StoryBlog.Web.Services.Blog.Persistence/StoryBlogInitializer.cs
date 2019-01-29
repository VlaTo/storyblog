using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryBlogInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public static void Seed(StoryBlogDbContext context, ILogger logger)
        {
            //const string email = "dev@storyblog.net";

            using (var transaction = context.Database.BeginTransaction())
            {

                if (false == context.Addresses.Any())
                {
                    context.Addresses.Add(new Address
                    {
                        AddressTypes = AddressTypes.Home,
                        City = "",
                        Country = "",
                        Street = "",
                        State = ""
                    });
                }

                transaction.Commit();
            }

            /*var customer = await customerManager.FindByEmailAsync(email);

            if (null == customer)
            {
                var result = await customerManager.CreateAsync(new Author
                {
                    UserName = "DevUser",
                    Email = email,
                    PhoneNumber = "+7(123)123-45-67"
                }, "Abcd1234!");

                if (false == result.Succeeded)
                {
                    if (environment.IsDevelopment())
                    {
                        logger.LogDebug("[StoryBlogIdentitySeed::SeedAsync] Failed to create customer");
                    }
                }
                else
                {
                    customer = await customerManager.FindByEmailAsync(email);
                }
            }

            if (customerManager.SupportsUserRole)
            {

            }

            if (customerManager.SupportsUserClaim)
            {

            }*/
        }
    }
}