using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Store.Models;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class StoryFeature : Feature<StoryState>
    {
        public override string GetName() => nameof(StoryState);

        protected override StoryState GetInitialState() => new StoryState(ModelStatus.None);
    }

    /// <summary>
    /// 
    /// </summary>
    public class StoryState : IHasModelStatus
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

        /// <summary>
        /// 
        /// </summary>
        public Author Author
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Closed
        {
            get;
            set;
        }

        public int AllCommentsCount
        {
            get;
            set;
        }

        public IList<Comment> Comments
        {
            get;
        }

        public StoryState(ModelStatus status)
        {
            Status = status;
            Comments = new List<Comment>();
        }
    }
}