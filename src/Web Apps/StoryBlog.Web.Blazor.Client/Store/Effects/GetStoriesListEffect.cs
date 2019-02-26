using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using System;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.Interop.Includes;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListEffect : Effect<GetStoriesListAction>
    {
        private readonly IBlogApiClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public GetStoriesListEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(GetStoriesListAction action, IDispatcher dispatcher)
        {
            try
            {
                var result = await client.GetStoriesAsync(StoryIncludes.Comments);

                if (null == result)
                {
                    dispatcher.Dispatch(new GetStoriesListFailedAction(""));
                    return;
                }

                dispatcher.Dispatch(new GetStoriesListSuccessAction(result.Data));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoriesListFailedAction(exception.Message));
            }
        }
    }
}
