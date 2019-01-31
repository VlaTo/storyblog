using Microsoft.AspNetCore.Routing;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NavigationCursorParameterTransformer : IOutboundParameterTransformer
    {
        /// <inheritdoc cref="IOutboundParameterTransformer.TransformOutbound" />
        public string TransformOutbound(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}