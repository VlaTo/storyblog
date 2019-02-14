using System.Linq;
using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Common.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    public sealed class BlogStoriesFeature : Feature<BlogState>
    {
        public override string GetName() => nameof(BlogState);

        protected override BlogState GetInitialState() => new BlogState(false, Enumerable.Empty<StoryModel>(), null);
    }
}