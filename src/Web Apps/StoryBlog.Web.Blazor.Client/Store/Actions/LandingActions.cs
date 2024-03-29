﻿using Blazor.Fluxor;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Store.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetLandingAction : IAction
    {
        public LandingIncludes Includes
        {
            get;
        }

        public GetLandingAction(LandingIncludes includes)
        {
            Includes = includes;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetLandingSuccessAction : IAction
    {
        public LandingModel Data
        {
            get;
        }

        public GetLandingSuccessAction(LandingModel data)
        {
            Data = data;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class GetLandingFailedAction : IAction
    {
        public string Error
        {
            get;
        }

        public GetLandingFailedAction(string error)
        {
            Error = error;
        }
    }
}