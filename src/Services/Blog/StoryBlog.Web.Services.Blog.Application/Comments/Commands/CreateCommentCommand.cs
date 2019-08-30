using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public enum CreateCommentFailedReason
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown,

        /// <summary>
        /// 
        /// </summary>
        NoStoryFound,

        /// <summary>
        /// 
        /// </summary>
        StoryNotPublished,

        /// <summary>
        /// 
        /// </summary>
        CommentsClosed,

        /// <summary>
        /// 
        /// </summary>
        SpecifiedParentNotFound
    }

    /// <summary>
    /// 
    /// </summary>
    public struct CreateCommentResult
    {
        public bool IsSuccess { get; }

        public bool IsFailed { get; }

        public CreateCommentFailedReason Reason { get; }

        public Comment Comment { get; }

        public CreateCommentResult(bool isSuccess, Comment comment = null, bool isFailed = false, CreateCommentFailedReason reason=CreateCommentFailedReason.Unknown)
        {
            IsSuccess = isSuccess;
            IsFailed = isFailed;
            Comment = comment;
            Reason = reason;
        }

        public static CreateCommentResult Success(Comment comment)
        {
            return new CreateCommentResult(true, comment: comment);
        }

        public static CreateCommentResult Failed(CreateCommentFailedReason reason)
        {
            return new CreateCommentResult(false, isFailed: true, reason: reason);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateCommentCommand : IRequest<CreateCommentResult>
    {
        /// <summary>
        /// 
        /// </summary>
        public IPrincipal User
        {
            get;
        }

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
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPublic
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="slug"></param>
        /// <param name="parentId"></param>
        /// <param name="content"></param>
        /// <param name="isPublic"></param>
        public CreateCommentCommand(IPrincipal user, string slug, long? parentId, string content, bool isPublic)
        {
            User = user;
            Slug = slug;
            ParentId = parentId;
            Content = content;
            IsPublic = isPublic;
        }
    }
}
