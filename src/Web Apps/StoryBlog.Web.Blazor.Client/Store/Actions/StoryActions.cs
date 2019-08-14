﻿using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using System;

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
        public Story Story
        {
            get;
            set;
        }

        public GetStorySuccessAction(Story story)
        {
            if (null == story)
            {
                throw new ArgumentNullException(nameof(story));
            }

            Story = story;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateNewCommentAction
    {
        public string Slug
        {
            get;
        }

        public string Text
        {
            get;
        }

        public CreateNewCommentAction(string slug, string text)
        {
            Slug = slug;
            Text = text;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreatedAction
    {
        public string Slug
        {
            get;
        }

        public CommentCreated Comment
        {
            get;
        }

        public CommentCreatedAction(string slug, CommentCreated comment)
        {
            Slug = slug;
            Comment = comment;
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