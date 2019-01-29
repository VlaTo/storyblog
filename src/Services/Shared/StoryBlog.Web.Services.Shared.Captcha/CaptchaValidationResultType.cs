namespace StoryBlog.Web.Services.Shared.Captcha
{
    internal enum ValidationResult
    {
        NotExists = -2,
        Expired,
        Success,
        Missmatch
    }
}
