using Microsoft.Extensions.ObjectPool;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public class CaptchaSerializationContextPooledObjectPolicy : IPooledObjectPolicy<CaptchaSerializationContext>
    {
        public CaptchaSerializationContext Create()
        {
            throw new System.NotImplementedException();
        }

        public bool Return(CaptchaSerializationContext obj)
        {
            throw new System.NotImplementedException();
        }
    }
}