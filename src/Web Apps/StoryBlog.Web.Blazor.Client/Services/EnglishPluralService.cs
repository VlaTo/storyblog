using System.Globalization;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal class EnglishPluralService : PluralService
    {
        public EnglishPluralService() 
            : base(CultureInfo.GetCultureInfo("en-us"))
        {
        }

        protected override int GetResourcePluralIndex(long numerator)
        {
            return 0 == numerator ? 0 : 1 == numerator ? 1 : 2;
        }
    }
}