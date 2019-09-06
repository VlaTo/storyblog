using System.Globalization;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal class RussianPluralService : PluralService
    {
        public RussianPluralService()
            : base(CultureInfo.GetCultureInfo("ru-ru"))
        {
        }

        protected override int GetResourcePluralIndex(long numerator)
        {
            // c%10==1 && c%100!=11 ? 1 : c%10>=2 && c%10<=4 && (c%100<10 || c%100>=20) ? 2 : 3

            if (0 == numerator)
            {
                return 0;
            }

            return 1 == (numerator % 10) && 11 != (numerator % 100) ? 1 : (numerator % 10) >= 2 && (numerator % 10) <= 4 && ((numerator % 100) < 10 || (numerator % 100) >= 20) ? 2 : 3;

            /*if (1 == numerator)
            {
                return 1;
            }

            var twoDigits = (int) numerator % 100;

            if (10 < twoDigits && 20 > twoDigits)
            {
                return 3;
            }

            var lastDigit = (int)(numerator % 10);

            if (1 == lastDigit)
            {
                return 4;
            }

            return 1 < lastDigit && 5 > lastDigit ? 2 : 3;*/
        }
    }
}