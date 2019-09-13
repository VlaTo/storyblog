using Blazor.Fluxor;
using StoryBlog.Web.Client.Store.Models;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Client.Store
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
        /// <summary>
        /// 
        /// </summary>
        public ModelStatus Status
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Slug
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
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
        public DateTime Published
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCommentsClosed
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<CommentBase> Comments
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets all comments count.
        /// </summary>
        public int CommentsCount
        {
            get; 
            set;
        }

        public StoryState(ModelStatus status)
        {
            Status = status;
            Comments = new CommentBase[0];
        }
    }
}