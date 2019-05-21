using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesActionEffect : Effect<GetStoriesAction>
    {
        private readonly IBlogApiClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public GetStoriesActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="Effect{TTriggerAction}.HandleAsync" />
        protected override async Task HandleAsync(GetStoriesAction action, IDispatcher dispatcher)
        {
            try
            {
                var result = await client.GetStoriesAsync(action.Includes);

                if (null == result)
                {
                    throw new Exception("No result received");
                }

                dispatcher.Dispatch(new GetStoriesSuccessAction(result, new Navigation()));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoriesFailedAction(exception.Message));
            }
        }
    }
}
