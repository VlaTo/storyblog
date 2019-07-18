using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesAction : IAction
    {
        public StoryFlags Flags
        {
            get;
        }

        public GetStoriesAction(StoryFlags flags)
        {
            Flags = flags;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesBackwardAction : IAction
    {
        public Uri RequestUri
        {
            get;
        }

        public GetStoriesBackwardAction(Uri requestUri)
        {
            RequestUri = requestUri;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesForwardAction : IAction
    {
        public Uri RequestUri
        {
            get;
        }

        public GetStoriesForwardAction(Uri requestUri)
        {
            RequestUri = requestUri;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetRubricStoriesAction : IAction
    {
        public StoryFlags Flags
        {
            get;
        }

        public string Rubric
        {
            get;
        }

        public GetRubricStoriesAction(string rubric, StoryFlags flags)
        {
            Rubric = rubric;
            Flags = flags;
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

        public Uri BackwardUri
        {
            get;
        }

        public Uri ForwardUri
        {
            get;
        }

        public GetStoriesSuccessAction(IEnumerable<FeedStory> stories, Uri backwardUri = null, Uri forwardUri = null)
        {
            Stories = stories;
            BackwardUri = backwardUri;
            ForwardUri = forwardUri;
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
