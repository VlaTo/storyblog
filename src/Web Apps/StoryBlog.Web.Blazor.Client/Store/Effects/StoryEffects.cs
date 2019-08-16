﻿using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal sealed class GetStoryActionEffect : Effect<GetStoryAction>
    {
        private readonly IBlogApiClient client;

        public GetStoryActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(GetStoryAction action, IDispatcher dispatcher)
        {
            try
            {
                var story = await client.GetStoryAsync(action.Slug, action.Flags, CancellationToken.None);

                if (null == story)
                {
                    throw new Exception("");
                }

                var result = new GetStorySuccessAction
                {
                    Slug = story.Slug,
                    Title = story.Title,
                    Author = story.Author,
                    Content = story.Content,
                    Published = story.Published,
                    Comments = story.Comments
                };

                dispatcher.Dispatch(result);
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoryFailedAction(exception.Message));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal sealed class CreatePendingCommentActionEffect : Effect<CreatePendingCommentAction>
    {
        private readonly IBlogApiClient client;

        public CreatePendingCommentActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(CreatePendingCommentAction action, IDispatcher dispatcher)
        {
            try
            {
                var result = await client.CreateCommentAsync(action.StorySlug,  action.ParentId, action.Content, CancellationToken.None);

                if (null == result)
                {
                    throw new Exception("");
                }

                dispatcher.Dispatch(new CommentCreatedAction(action.StorySlug)
                {
                    Id = result.Id,
                    Author = result.Author,
                    ParentId = result.Parent,
                    Content = result.Content,
                    Published = result.Published.ToLocalTime()
                });
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new CommentCreationFailedAction(action.StorySlug, exception.Message));
            }
        }
    }
}