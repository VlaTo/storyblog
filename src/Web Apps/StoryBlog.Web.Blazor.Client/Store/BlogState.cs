using StoryBlog.Web.Services.Blog.Interop.Models;
using System.Collections.Generic;
using System.Linq;
using Blazor.Fluxor;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BlogStoriesFeature : Feature<BlogState>
    {
        public override string GetName() => nameof(BlogState);

        protected override BlogState GetInitialState() =>
            new BlogState(false, Enumerable.Empty<StoryModel>(), null);
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class BlogState
    {
        public bool IsBusy
        {
            get;
        }
        
        public IEnumerable<StoryModel> Stories
        {
            get;
        }

        public string Error
        {
            get;
        }

        public BlogState(bool isBusy, IEnumerable<StoryModel> stories, string error)
        {
            IsBusy = isBusy;
            Stories = stories;
            Error = error;
        }
    }
}