using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            SeedSettings(context, logger);

            var johnDoe = CreateAuthor(context, "John Doe");
            var richardRoe = CreateAuthor(context, "Richard Roe");
            var janeRoe = CreateAuthor(context, "Jane Roe");
            var jimSmith = CreateAuthor(context, "Jim Smith");

            /*using (var transaction = context.Database.BeginTransaction())
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
            }*/

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

        private static void SeedSettings(StoryBlogDbContext context, ILogger logger)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                AddSetting(context, "Landing.Title", "The Story Blog");
                AddSetting(context, "Landing.Description", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.");

                transaction.Commit();
            }
        }

        private static Author CreateAuthor(StoryBlogDbContext context, string userName)
        {
            var author = context.Authors.AsNoTracking().SingleOrDefault(entity => entity.UserName == userName);

            if (null != author)
            {
                return author;
            }

            author = new Author
            {
                UserName = userName
            };

            using (var transaction = context.Database.BeginTransaction())
            {
                context.Authors.Add(author);
                context.SaveChanges();
                transaction.Commit();
            }

            return author;
        }

        private static void AddSetting(StoryBlogDbContext context, string name, string value)
        {
            if (context.Settings.Any(setting => setting.Name == name))
            {
                return;
            }

            context.Settings.Add(new Settings
            {
                Name = name,
                Value = Encoding.UTF8.GetBytes(value)
            });

            context.SaveChanges();
        }
    }
}