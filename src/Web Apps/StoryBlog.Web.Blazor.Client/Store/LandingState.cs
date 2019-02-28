﻿using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Blazor.Client.Store
{
    public sealed class LandingState
    {
        public static readonly LandingState Empty;

        public bool IsBusy
        {
            get;
        }

        public LandingModel Model
        {
            get;
        }

        public string Error
        {
            get;
        }

        private LandingState(bool isBusy, LandingModel model, string error)
        {
            IsBusy = isBusy;
            Model = model;
            Error = error;
        }

        static LandingState()
        {
            Empty = new LandingState(false, null, null);
        }

        public static LandingState Loading(LandingModel model)
        {
            return new LandingState(true, model, null);
        }

        public static LandingState Success(LandingModel model)
        {
            return new LandingState(false, model, null);
        }

        public static LandingState Failed(LandingModel model, string error)
        {
            return new LandingState(false, model, error);
        }
    }
}