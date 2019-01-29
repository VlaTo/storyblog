using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Services.Shared.Common.ActionResults
{
    /// <summary>
    /// 
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public InternalServerErrorObjectResult(object value)
            : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}