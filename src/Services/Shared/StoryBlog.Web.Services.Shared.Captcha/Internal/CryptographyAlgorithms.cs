using System.Reflection;
using System.Security.Cryptography;

namespace StoryBlog.Web.Services.Shared.Captcha.Internal
{
    internal static class CryptographyAlgorithms
    {
        internal static SHA256 CreateSHA256()
        {
            try
            {
                return SHA256.Create();
            }
            catch (TargetInvocationException e)
            {
                return new SHA256CryptoServiceProvider();
            }
        }
    }
}