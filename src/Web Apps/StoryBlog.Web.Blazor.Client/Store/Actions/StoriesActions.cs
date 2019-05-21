using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Shared.Common;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesAction : IAction
    {
        public StoryIncludes Includes
        {
            get;
        }

        public GetStoriesAction(StoryIncludes includes)
        {
            Includes = includes;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetRubricStoriesAction : IAction
    {
        public StoryIncludes Includes
        {
            get;
        }

        public string Rubric
        {
            get;
        }

        public GetRubricStoriesAction(string rubric, StoryIncludes includes)
        {
            Rubric = rubric;
            Includes = includes;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesSuccessAction : IAction
    {
        public IEnumerable<FeedStory> Stories
        {
            get;
        }

        public Navigation Navigation
        {
            get;
        }

        public GetStoriesSuccessAction(IEnumerable<FeedStory> stories, Navigation navigation)
        {
            Stories = stories;
            Navigation = navigation;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesFailedAction : IAction
    {
        public string Error
        {
            get;
        }

        public GetStoriesFailedAction(string error)
        {
            Error = error;
        }
    }
}
