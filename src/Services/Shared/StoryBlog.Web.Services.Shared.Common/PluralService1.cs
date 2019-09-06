namespace StoryBlog.Web.Services.Shared.Common
{
    public class PluralService1 : IPluralService
    {
        private readonly Pluralizer.Pluralizer pluralizer;

        public string this[string noun, long numerator]
        {
            get
            {
                if (1 == numerator)
                {
                    return pluralizer.Singularize(noun);
                }

                return pluralizer.Pluralize(noun);
            }
        }

        public PluralService1()
        {
            pluralizer = new Pluralizer.Pluralizer();
        }
    }
}