using System;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal abstract class ApiOptionsBase
    {
        public abstract Uri Host
        {
            get;
            set;
        }
    }
}