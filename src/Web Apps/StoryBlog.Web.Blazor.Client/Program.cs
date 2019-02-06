using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Blazor.Client.Core;
using StoryBlog.Web.Blazor.Shared;

namespace StoryBlog.Web.Blazor.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            var host = BlazorWebAssemblyHost
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.Add(ServiceDescriptor.Singleton<IApiClient, ApiClient>());
                    services.AddLocalStorage();
                    /*services.AddFluxor(options =>
                    {
                        options.UseDependencyInjection(typeof(Program).Assembly);
                    });*/
                    services
                        .AddMvc()
                        .AddNewtonsoftJson();
                })
                .UseBlazorStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
