using System;
using Blazor.Fluxor;
using IdentityModel;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Blazor.Client.Services;

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
                        .AddOptions<UserApiClientOptions>()
                        .Configure(options =>
                        {
                            options.Address = "http://localhost:3100";
                            options.ClientId = "client.application";
                            options.Scopes = new[]
                            {
                                OidcConstants.StandardScopes.OpenId,
                                OidcConstants.StandardScopes.Profile,
                                "api.blog"
                            };
                        });

                    services
                        .AddSingleton<IBlogApiClient, BlogApiClient>()
                        .AddSingleton<IUserApiClient, UserApiClient>();
                })
                .UseBlazorStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}