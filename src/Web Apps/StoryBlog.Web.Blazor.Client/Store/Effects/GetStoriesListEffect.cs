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
    public sealed class GetStoriesListEffect : Effect<GetStoriesAction>
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

        protected override async Task HandleAsync(GetStoriesAction action, IDispatcher dispatcher)
        {
            try
            {
                var result = await client.GetStoriesAsync(action.Includes);

                if (null == result)
                {
                    throw new Exception("No result received");
                }

                dispatcher.Dispatch(new GetStoriesSuccessAction(result.Data));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoriesFailedAction(exception.Message));
            }
        }
    }
}
