using System;
using System.Security.Cryptography;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    internal sealed class CaptchaTextGenerator
    {
        private readonly string allowedChars;
        private readonly int captchaLength;
        private readonly RandomNumberGenerator numberGenerator;

        public CaptchaTextGenerator(string allowedChars, int captchaLength)
        {
            this.allowedChars = allowedChars;
            this.captchaLength = captchaLength;
            numberGenerator = RandomNumberGenerator.Create();
        }

        public char[] Generate()
        {
            var length = allowedChars.Length;
            var buffer = new Span<char>(new char[captchaLength]);
            var numbers = GetNumbers().ToArray();

            for (var position = 0; position < buffer.Length; position++)
            {
                var index = numbers[position] % length;
                buffer[position] = allowedChars[index];
            }

            return buffer.ToArray();
        }

        private Memory<byte> GetNumbers()
        {
            var numbers = new Span<byte>(new byte[captchaLength]);

            numberGenerator.GetBytes(numbers);

            return new Memory<byte>(numbers.ToArray());
        }
    }
}