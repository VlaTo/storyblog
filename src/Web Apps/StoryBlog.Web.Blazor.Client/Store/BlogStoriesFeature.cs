using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System.Linq;

namespace StoryBlog.Web.Blazor.Client.Store
{
    public sealed class BlogStoriesFeature : Feature<BlogState>
    {
        public override string GetName() => nameof(BlogState);

        protected override BlogState GetInitialState() =>
            new BlogState(false, Enumerable.Empty<StoryModel>(), null);
    }
}