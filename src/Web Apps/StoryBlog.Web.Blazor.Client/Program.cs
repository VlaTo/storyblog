using Blazor.Fluxor;
using IdentityModel;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Blazor.Client.Core;
using StoryBlog.Web.Blazor.Client.Extensions;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Globalization;

namespace StoryBlog.Web.Blazor.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BlazorWebAssemblyHost.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddFluxor(options =>
                        options.UseDependencyInjection(typeof(Program).Assembly)
                    );

                    services
                        .AddOptions<IdentityApiOptions>()
                        .Configure(options =>
                        {
                            options.Host = new Uri("http://localhost:3100");
                            options.Address = "http://localhost:3100";
                            options.ClientId = "client.application";
                            options.RedirectUri = "http://localhost:62742/callback";
                            options.Scopes = new[]
                            {
                                OidcConstants.StandardScopes.OpenId,
                                OidcConstants.StandardScopes.Profile,
                                "api.blog"
                            };
                        });

                    services
                        .AddOptions<BlogApiOptions>()
                        .Configure(options =>
                        {
                            options.Host = new Uri("http://localhost:3000/api/v1/");
                        });

                    services
                        .AddSingleton<IBlogApiClient, BlogApiClient>()
                        .AddSingleton<IIdentityApiClient, IdentityApiClient>()
                        .AddSingleton<ITimeSpanLocalizer, TimeSpanLocalizer>()
                        .AddSingleton<IDateTimeLocalizer, DateTimeLocalizer>()
                        .AddSingleton(serviceProvider =>
                        {
                            var culture = CultureInfo.CurrentUICulture;
                            return PluralServiceFactory.Instance.GetService(culture);
                        });

                    services
                        .AddSingleton<IIdManager, IdManager>()
                        .AddBootstrapModalService();
                })
                .UseBlazorStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}