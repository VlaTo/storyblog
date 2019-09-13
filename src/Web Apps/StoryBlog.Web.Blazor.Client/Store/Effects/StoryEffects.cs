using Blazor.Fluxor;
using StoryBlog.Web.Client.Services;
using StoryBlog.Web.Client.Store.Actions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Store.Effects
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
    internal sealed class SaveReplyActionEffect : Effect<SaveReplyAction>
    {
        private readonly IBlogApiClient client;

        public SaveReplyActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(SaveReplyAction action, IDispatcher dispatcher)
        {
            //Debug.WriteLine($"[SaveReplyActionEffect.HandleAsync] Saving comment for: \'{action.StorySlug}\' parent: {action.ParentId} ref: {action.Reference}");
            try
            {
                var result = await client.CreateCommentAsync(action.StorySlug,  action.ParentId, action.Content, CancellationToken.None);

                if (null == result)
                {
                    throw new Exception("");
                }

                //Debug.WriteLine($"[SaveReplyActionEffect.HandleAsync] Dispatching action: \'{nameof(ReplyPublishedAction)}\'");

                dispatcher.Dispatch(new ReplyPublishedAction(action.StorySlug)
                {
                    Id = result.Id,
                    Author = result.Author,
                    ParentId = result.Parent,
                    Reference = action.Reference,
                    Content = result.Content,
                    Published = result.Published.ToLocalTime()
                });
            }
            catch (Exception exception)
            {
                //Debug.WriteLine($"[SaveReplyActionEffect.HandleAsync] Failed: \"{exception}\"");
                dispatcher.Dispatch(new CommentCreationFailedAction(action.StorySlug, exception.Message));
            }
        }
    }
}