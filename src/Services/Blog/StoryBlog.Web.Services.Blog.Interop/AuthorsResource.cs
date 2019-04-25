using System.Collections.Generic;
using System.Runtime.Serialization;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Services.Blog.Interop
{
    [DataContract(IsReference = false, Name = "resources", Namespace = "http://storyblog.org/schemas/json/result/resources")]
    public sealed class AuthorsResource : ListResultResources
    {
        [DataMember(Name = "authors")]
        public IEnumerable<AuthorModel> Authors
        {
            get;
            set;
        }
    }
}