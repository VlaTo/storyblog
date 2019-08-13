using System;
using Blazor.Fluxor;
using Microsoft.AspNetCore.Components;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Controls
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void SubmitCommentEventHandler(CommentBoxBase sender, SubmitCommentEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    public sealed class SubmitCommentEventArgs : EventArgs
    {
        public string Message
        {
            get;
        }

        public SubmitCommentEventArgs(string message)
        {
            Message = message;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    //[EventHandler("e", typeof(SubmitCommentEventArgs))]
    public class CommentBoxBase : ComponentBase
    {
        [Inject]
        protected IDispatcher Dispatcher
        {
            get;
            set;
        }

        [Inject]
        protected IPluralLocalizer Pluralizer
        {
            get;
            set;
        }

        [Parameter]
        public long? ParentId
        {
            get;
            set;
        }

        [Parameter]
        protected string Message
        {
            get;
            set;
        }

        [Parameter]
        protected EventCallback<string> OnSubmitComment
        {
            get;
            set;
        }

        protected void OnSubmitButtonClick(UIMouseEventArgs e)
        {
            OnSubmitComment.InvokeAsync(Message);
        }
    }
}