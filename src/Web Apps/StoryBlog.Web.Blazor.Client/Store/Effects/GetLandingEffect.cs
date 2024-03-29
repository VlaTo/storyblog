﻿using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetLandingEffect : Effect<GetLandingAction>
    {
        private readonly IBlogApiClient client;

        public GetLandingEffect(IBlogApiClient client)
        {
            this.client = client;
        }

        protected override async Task HandleAsync(GetLandingAction action, IDispatcher dispatcher)
        {
            try
            {
                var landing = await client.GetLandingAsync(action.Includes);

                if (null == landing)
                {
                    throw new Exception("");
                }

                dispatcher.Dispatch(new GetLandingSuccessAction(landing));
            }
            catch (Exception exception)
            {
                dispatcher.Dispatch(new GetLandingFailedAction(exception.Message));
            }
        }
    }
}