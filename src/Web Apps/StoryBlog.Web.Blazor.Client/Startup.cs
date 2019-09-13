using Microsoft.AspNetCore.Components.Builder;

namespace StoryBlog.Web.Client
{
    public sealed class Startup
    {
        public void Configure(IComponentsApplicationBuilder app) => app.AddComponent<App>("app");
    }
}