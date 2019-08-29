using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    public class BBCodeNode : MarkupNode
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class TagNode : MarkupNode
    {
        public string Name
        {
            get;
        }

        public string Value
        {
            get;
        }
        
        public IList<MarkupNode> Inlines
        {
            get;
        }

        public TagNode(string name, string value = null)
        {
            Name = name;
            Value = value;
            Inlines = new List<MarkupNode>();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TextNode : MarkupNode
    {
        public string Text
        {
            get;
        }

        public TextNode(string text)
        {
            Text = text;
        }
    }
}