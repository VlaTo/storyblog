﻿using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using System;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    internal interface IBlogApiClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<EntityListResult<FeedStory>> GetStoriesAsync(StoryFlags flag, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<EntityListResult<FeedStory>> GetStoriesAsync(Uri requestUri, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Story> GetStoryAsync(string slug, StoryFlags flags, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="parentId"></param>
        /// <param name="text"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CommentCreated> CreateCommentAsync(string slug, long? parentId, string text, CancellationToken cancellationToken);
    }
}