using System.Drawing;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICaptchaImageGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="captcha"></param>
        /// <returns></returns>
        Task<Image> GenerateImageAsync(char[] captcha);
    }
}