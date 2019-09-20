using System.Collections.Specialized;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HtmlTag
    {
        public string TagName
        {
            get;
        }

        public NameValueCollection Attributes
        {
            get;
        }

        public HtmlTag(string tagName)
        {
            TagName = tagName;
            Attributes = new NameValueCollection();
        }

        public void WriteOpen(HtmlWriter writer)
        {
            writer.WriteTagStart(TagName);

            foreach (string key in Attributes.Keys)
            {
                var value = Attributes[key];
                writer.WriteAttribute(key, value);
            }

            writer.WriteTagClose(false);
        }

        public void WriteClose(HtmlWriter writer)
        {
            writer.WriteTagEnd(TagName);
        }
    }
}