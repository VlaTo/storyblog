using System.Collections.Generic;
using LibraProgramming.Web.Mvc;

namespace StoryBlog.Web.Services.Shared.Mvc.Abstraction
{
    internal interface IHtmlElementBuilder
    {
        IList<IHtmlAdorner> Adorners
        {
            get;
        }

        IHtmlElement Build();
    }
}