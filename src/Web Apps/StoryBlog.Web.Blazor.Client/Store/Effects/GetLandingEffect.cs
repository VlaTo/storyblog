﻿namespace StoryBlog.Web.Client.Store.Effects
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    /*internal sealed class GetLandingEffect : Effect<GetLandingAction>
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
                var landing = await client.GetLandingAsync(action.Includes, CancellationToken.None);

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
    }*/
}