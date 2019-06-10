using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    public struct StoryRequestResult : RequestResult<Story>, IRequestResult<Story>
    {

    }
}
