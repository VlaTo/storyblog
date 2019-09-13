using System;

namespace StoryBlog.Web.Client.Services
{
    internal sealed class BlogApiOptions : ApiOptionsBase
    {
        private Uri host;

        public override Uri Host
        {
            get => host;
            set
            {
                if (Uri.Equals(host, value))
                {
                    return;
                }

                host = value;
            }
        }
    }
}