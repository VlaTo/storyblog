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
    // ReSharper disable once UnusedMember.Global
    internal sealed class GetStoriesActionEffect : Effect<GetStoriesAction>
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
                var result = await client.GetStoriesAsync(action.Flags, CancellationToken.None);

                if (null == result)
                {
                    throw new Exception("No result received");
                }

                dispatcher.Dispatch(new GetStoriesSuccessAction(result, result.BackwardUri, result.ForwardUri));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoriesFailedAction(exception.Message));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal sealed class GetStoriesBackwardActionEffect : Effect<GetStoriesBackwardAction>
    {
        private readonly IBlogApiClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public GetStoriesBackwardActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="Effect{TTriggerAction}.HandleAsync" />
        protected override async Task HandleAsync(GetStoriesBackwardAction action, IDispatcher dispatcher)
        {
            try
            {
                var result = await client.GetStoriesFromAsync(action.RequestUri, CancellationToken.None);

                if (null == result)
                {
                    throw new Exception("No result received");
                }

                dispatcher.Dispatch(new GetStoriesSuccessAction(result, result.BackwardUri, result.ForwardUri));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoriesFailedAction(exception.Message));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal sealed class GetStoriesForwardActionEffect : Effect<GetStoriesForwardAction>
    {
        private readonly IBlogApiClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public GetStoriesForwardActionEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="Effect{TTriggerAction}.HandleAsync" />
        protected override async Task HandleAsync(GetStoriesForwardAction action, IDispatcher dispatcher)
        {
            try
            {
                var result = await client.GetStoriesFromAsync(action.RequestUri, CancellationToken.None);

                if (null == result)
                {
                    throw new Exception("No result received");
                }

                dispatcher.Dispatch(new GetStoriesSuccessAction(result, result.BackwardUri, result.ForwardUri));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetStoriesFailedAction(exception.Message));
            }
        }
    }
}