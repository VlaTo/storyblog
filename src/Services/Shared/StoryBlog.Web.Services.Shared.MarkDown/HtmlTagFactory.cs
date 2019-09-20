using System;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public class HtmlTagFactory
    {
        public static HtmlTagFactory Instance
        {
            get;
        }

        private HtmlTagFactory()
        {
        }

        static HtmlTagFactory()
        {
            Instance = new HtmlTagFactory();
        }

        public HtmlTag CreateDiv()
        {
            return new HtmlTag("div");
        }

        public HtmlTag CreateHeading(int level)
        {
            if (1 > level || 8 < level)
            {
                throw new ArgumentException($"Wrong level: {level}", nameof(level));
            }

            var tagName = $"h{level}";

            return new HtmlTag(tagName);
        }
    }
}