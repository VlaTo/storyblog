using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryFeature : Feature<StoryState>
    {
        public override string GetName() => nameof(StoryState);

        protected override StoryState GetInitialState() => new StoryState(ModelStatus.None);
    }

    /// <summary>
    /// 
    /// </summary>
    public class StoryState
    {
        public ModelStatus Status
        {
            get;
        }

        public string Title
        {
            get;
            set;
        }

        public string Slug
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public DateTime Published
        {
            get;
            set;
        }

        public int AllCommentsCount
        {
            get;
            set;
        }

        public IList<CommentModel> Comments
        {
            get;
        }

        public StoryState(ModelStatus status)
        {
            Status = status;
            Comments = new List<CommentModel>();
        }
    }
}