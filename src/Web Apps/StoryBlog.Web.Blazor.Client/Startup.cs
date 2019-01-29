using Microsoft.AspNetCore.Blazor.Builder;

namespace StoryBlog.Web.Blazor.Client
{
    public class Startup
    {
        public void Configure(IBlazorApplicationBuilder builder)
        {
            builder.AddComponent<App>("app");
        }
    }
}