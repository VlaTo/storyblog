using System;
using System.Collections.Specialized;
using System.Net.Mail;

namespace StoryBlog.Web.Services.Identity.Application.Models
{
    public sealed class MailMessageTemplateContext
    {
        public MailAddress From
        {
            get;
            set;
        }

        public MailAddressCollection To
        {
            get;
        }

        public string Subject
        {
            get;
            set;
        }

        public NameValueCollection Replacements
        {
            get;
        }

        public MailMessageTemplateContext()
            : this(StringComparer.CurrentCulture)
        {
        }

        public MailMessageTemplateContext(StringComparer comparer)
        {
            To = new MailAddressCollection();
            Replacements = new NameValueCollection(comparer);
        }
    }
}