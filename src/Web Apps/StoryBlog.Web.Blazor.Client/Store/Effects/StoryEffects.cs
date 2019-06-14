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
    internal sealed class StoryEffect : Effect<GetStoryAction>
    {
        private readonly IBlogApiClient client;

        public StoryEffect(IBlogApiClient client)
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
}