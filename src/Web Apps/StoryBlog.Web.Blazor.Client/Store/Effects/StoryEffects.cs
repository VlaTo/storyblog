using Blazor.Fluxor;
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

                var result = new GetStorySuccessAction(story);

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
    internal sealed class CreateNewCommentActionEffect : Effect<CreateNewCommentAction>
    {
        private readonly IBlogApiClient client;

        public CreateNewCommentActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(CreateNewCommentAction action, IDispatcher dispatcher)
        {
            try
            {
                var comment = await client.CreateCommentAsync(action.Slug, null, action.Text, CancellationToken.None);

                if (null == comment)
                {
                    throw new Exception("");
                }

                var result = new CommentCreatedAction(action.Slug, comment);

                dispatcher.Dispatch(result);
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new CommentCreationFailedAction(action.Slug, exception.Message));
            }
        }
    }
}