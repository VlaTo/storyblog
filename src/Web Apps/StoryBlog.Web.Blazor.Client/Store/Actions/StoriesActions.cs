using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
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
        public IEnumerable<StoryModel> Stories
        {
            get;
        }

        public GetStoriesSuccessAction(IEnumerable<StoryModel> stories)
        {
            Stories = stories;
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
