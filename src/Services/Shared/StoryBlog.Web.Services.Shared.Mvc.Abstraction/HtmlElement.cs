using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StoryBlog.Web.Services.Shared.Mvc.Abstraction
{
    public class HtmlElement : IHtmlElement
    {
        private readonly TagBuilder builder;
        private Action<TextWriter> template;

        public string TagName => builder.TagName;

        public IHtmlContentBuilder InnerHtml
        {
            get
            {
                if (Children.Any())
                {
                    var buffer = new StringBuilder();

                    using (var writer = new StringWriter(buffer, CultureInfo.InvariantCulture))
                    {
                        foreach (var element in Children)
                        {
                            element.WriteTo(writer);
                        }
                    }

                    return buffer.ToString();
                }

                return builder.InnerHtml;
            }
        }

        public TagRenderMode RenderMode
        {
            get; 
            private set; 
        }

        public IList<IHtmlElement> Children
        {
            get; 
            private set;
        }

        public IDictionary<string, string> Attributes => builder.Attributes;

        public HtmlElement(string tagName)
            : this(tagName, TagRenderMode.Normal)
        {
        }

        public HtmlElement(string tagName, TagRenderMode renderMode)
        {
            builder = new TagBuilder(tagName);
            RenderMode = renderMode;
            Children = new List<IHtmlElement>();
        }

        public IHtmlElement Html(string value)
        {
            Children.Clear();
            Children.Add(new HtmlLiteral(value));

            return this;
        }

        public IHtmlElement Template(Action<TextWriter> value)
        {
            template = value;
            return this;
        }

        public void WriteTo(TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            if (RenderMode == TagRenderMode.SelfClosing)
            {
                var closingTag = builder.RenderSelfClosingTag();
                closingTag.WriteTo(output, HtmlEncoder.Default);
                //output.Write(builder.ToString(RenderMode));
                return;
            }

            var startTag = builder.RenderStartTag();
            startTag.WriteTo(output, HtmlEncoder.Default);
            //output.Write(builder.ToString(TagRenderMode.StartTag));

            if (template != null)
            {
                template(output);
            }
            else if (Children.Any())
            {
                //Children.ForEach(child => child.WriteTo(output));
                foreach (var element in Children)
                {
                    element.WriteTo(output);
                }
            }
            else
            {
                output.Write(builder.InnerHtml);
            }

            //output.Write(builder.ToString(TagRenderMode.EndTag));
            var endTag = builder.RenderEndTag();
            endTag.WriteTo(output, HtmlEncoder.Default);
        }

        public IHtmlElement AppendTo(IHtmlElement parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            parent.Children.Add(this);

            return this;
        }

        public IHtmlElement Attribute(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            builder.MergeAttribute(key, value);

            return this;
        }

        public IHtmlElement MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
        {
            builder.MergeAttributes(attributes);
            return this;
        }

        public IHtmlElement PrependTo(IHtmlElement parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            parent.Children.Insert(0, this);

            return this;
        }

        public override string ToString()
        {
            var buffer = new StringBuilder();

            using (var writer = new StringWriter(buffer, CultureInfo.CurrentCulture))
            {
                WriteTo(writer);
            }

            return buffer.ToString();
        }
    }
}