using System;
using System.Net.Http;
using Blazor.Extensions.Logging;
using Blazor.Fluxor;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Blazor.Client.Services;

namespace StoryBlog.Web.Blazor.Client
{
    // http://wrapbootstrap.com/preview/WB0C58SK5
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var host = BlazorWebAssemblyHost
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddLogging(options =>
                        options
                            .AddBrowserConsole()
                            .SetMinimumLevel(LogLevel.Trace)
                    );
                    services.AddFluxor(options =>
                        options.UseDependencyInjection(typeof(Program).Assembly)
                    );
                    services.AddSingleton<IBlogApiClient, BlogApiClient>();
                })
                .UseBlazorStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
