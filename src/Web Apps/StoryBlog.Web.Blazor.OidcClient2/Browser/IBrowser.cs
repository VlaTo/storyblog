using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.OidcClient2.Browser
{
    public interface IBrowser
    {
        /// <summary>
        /// Invokes the browser.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        Task<BrowserResult> InvokeAsync(BrowserOptions options);
    }
}