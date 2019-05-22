using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class GetStoryEffect : Effect<GetStoryAction>
    {
        private readonly IBlogApiClient client;

        public GetStoryEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(GetStoryAction action, IDispatcher dispatcher)
        {
            try
            {
                var story = await client.GetStoryAsync(action.Slug, action.Flags);

                if (null == story)
                {
                    throw new Exception("");
                }

                var result = new GetStorySuccessAction(story.Comments)
                {
                    Slug = story.Slug,
                    Title = story.Title,
                    Content = story.Content,
                    Closed = story.Closed,
                    //Author = story.Author,
                    //Published = story.Modified.GetValueOrDefault(story.Created),
                };

                dispatcher.Dispatch(result);
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoryFailedAction(exception.Message));
            }
        }
    }
}