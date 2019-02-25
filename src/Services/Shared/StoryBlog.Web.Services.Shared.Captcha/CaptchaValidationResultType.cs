namespace StoryBlog.Web.Services.Shared.Captcha
{
    public enum ValidationResult
    {
        /// <summary>
        /// 
        /// </summary>
        NotExists = -2,

        /// <summary>
        /// 
        /// </summary>
        Expired,

        /// <summary>
        /// 
        /// </summary>
        Success,

        /// <summary>
        /// 
        /// </summary>
        Mismatch
    }
}
