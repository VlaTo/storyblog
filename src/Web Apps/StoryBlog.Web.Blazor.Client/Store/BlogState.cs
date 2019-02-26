using StoryBlog.Web.Services.Blog.Interop.Models;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store
{
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