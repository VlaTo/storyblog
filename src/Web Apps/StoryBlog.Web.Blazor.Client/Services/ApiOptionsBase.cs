using System;

namespace StoryBlog.Web.Client.Services
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