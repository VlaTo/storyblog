using Microsoft.AspNetCore.Components.Builder;

namespace StoryBlog.Web.Blazor.Client
{
    public sealed class Startup
    {
        public void Configure(IComponentsApplicationBuilder app) => app.AddComponent<App>("app");
    }
}
