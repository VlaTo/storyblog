using Blazor.Fluxor;
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
                        .AddSingleton<IBlogApiClient, BlogApiClient>()
                        .AddSingleton<IUserApiClient, UserApiClient>();
                })
                .UseBlazorStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
