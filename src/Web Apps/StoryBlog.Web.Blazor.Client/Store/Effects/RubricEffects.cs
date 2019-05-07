using System;
using System.Threading.Tasks;
using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetRubricsActionEffect : Effect<GetRubricsAction>
    {
        private readonly IBlogApiClient client;

        public GetRubricsActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(GetRubricsAction action, IDispatcher dispatcher)
        {
            try
            {
                var rubrics = await client.GetRubricsAsync();

                if (null == rubrics)
                {
                    throw new Exception("");
                }

                dispatcher.Dispatch(new GetRubricsSuccessAction(rubrics));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetLandingFailedAction(exception.Message));
            }
        }
    }
}