﻿using Blazor.Fluxor;
using StoryBlog.Web.Blazor.Client.Services;
using StoryBlog.Web.Blazor.Client.Store.Actions;
using System.Net.Http;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SigninRequestActionEffect : Effect<SigninRequestAction>
    {
        private readonly IUserApiClient client;

        public SigninRequestActionEffect(IUserApiClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="Effect{TTriggerAction}.HandleAsync" />
        protected override async Task HandleAsync(SigninRequestAction action, IDispatcher dispatcher)
        {
            try
            {
                await client.SigninAsync();
            }
            catch (HttpRequestException exception)
            {
                dispatcher.Dispatch(new SigninRequestFailedAction(exception.Message));
            }
        }
    }
}