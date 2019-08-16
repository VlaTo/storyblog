using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryAction
    {
        /// <summary>
        /// 
        /// </summary>
        public string Slug
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public StoryFlags Flags
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        public GetStoryAction(string slug, StoryFlags flags)
        {
            Slug = slug;
            Flags = flags;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryFailedAction
    {
        /// <summary>
        /// 
        /// </summary>
        public string Error
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        public GetStoryFailedAction(string error)
        {
            Error = error;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStorySuccessAction
    {
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
        public ICollection<Models.Data.Comment> Comments
        {
            get;
            set;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class CreatePendingCommentAction
    {
        /// <summary>
        /// 
        /// </summary>
        public string StorySlug
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public long? ParentId
        {
            get;
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
        /// <param name="storySlug"></param>
        /// <param name="parentId"></param>
        /// <param name="content"></param>
        public CreatePendingCommentAction(string storySlug, long? parentId, string content)
        {
            StorySlug = storySlug;
            ParentId = parentId;
            Content = content;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreatedAction
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string StorySlug
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public long? ParentId
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

        public CommentCreatedAction(string storySlug)
        {
            StorySlug = storySlug;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreationFailedAction
    {
        public string Slug
        {
            get;
        }

        public string Error
        {
            get;
        }

        public CommentCreationFailedAction(string slug, string error)
        {
            Slug = slug;
            Error = error;
        }
    }
}