using Microsoft.AspNetCore.Builder;

namespace StoryBlog.Web.Blazor.Client
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseBlazor<App>();
            app.UseBlazorDebugging();
        }
    }
}