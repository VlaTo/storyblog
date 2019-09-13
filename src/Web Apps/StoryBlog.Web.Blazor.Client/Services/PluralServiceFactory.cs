using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace StoryBlog.Web.Client.Services
{
    internal class PluralServiceFactory
    {
        private readonly IDictionary<CultureInfo, Func<CultureInfo, IPluralService>> registration;

        public static PluralServiceFactory Instance
        {
            get;
        }

        static PluralServiceFactory()
        {
            Instance = new PluralServiceFactory();
        }

        private PluralServiceFactory()
        {
            registration = new Dictionary<CultureInfo, Func<CultureInfo, IPluralService>>();
            registration.Add(CultureInfo.GetCultureInfo("en"), culture => new EnglishPluralService());
            registration.Add(CultureInfo.GetCultureInfo("ru-ru"), culture => new RussianPluralService());
        }

        public IPluralService GetService(CultureInfo culture)
        {
            if (null == culture)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            if (registration.TryGetValue(culture, out var creator))
            {
                return creator.Invoke(culture);
            }

            Debug.WriteLine($"Service registration for \'{culture.Name}\' was not found, returning neutral");

            return PluralService.Neutral;
        }
    }
}