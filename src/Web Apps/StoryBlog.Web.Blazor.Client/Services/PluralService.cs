using StoryBlog.Web.Services.Shared.Common;
using System.Globalization;
using System.Resources;

namespace StoryBlog.Web.Client.Services
{
    public abstract class PluralService : IPluralService
    {
        private readonly ResourceManager resourceManager;
        private CultureInfo culture;

        public static PluralService Neutral
        {
            get;
        }

        public string this[string noun, long numerator]
        {
            get
            {
                return resourceManager.GetString($"{noun}{GetResourcePluralIndex(numerator)}", Culture);
            }
        }

        private CultureInfo Culture
        {
            get
            {
                if (null == culture)
                {
                    culture = CultureInfo.CurrentUICulture;
                }

                return culture;
            }
            set => culture = value;
        }

        protected PluralService(CultureInfo culture)
        {
            resourceManager = new ResourceManager("StoryBlog.Web.Client.Resources", typeof(PluralService).Assembly);
            Culture = culture;
        }

        static PluralService()
        {
            Neutral = new NeutralPluralService();
        }

        protected abstract int GetResourcePluralIndex(long numerator);

        /// <summary>
        /// 
        /// </summary>
        private sealed class NeutralPluralService : PluralService
        {
            public NeutralPluralService()
                : base(CultureInfo.InvariantCulture)
            {
            }

            protected override int GetResourcePluralIndex(long numerator) => 0;
        }
    }
}