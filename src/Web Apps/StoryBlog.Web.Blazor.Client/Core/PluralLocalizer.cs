using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class PluralLocalizer : IPluralLocalizer
    {
        public string this[string noun, int numerator]
        {
            get
            {
                return noun;
            }
        }

        public PluralLocalizer()
        {
        }
    }
}