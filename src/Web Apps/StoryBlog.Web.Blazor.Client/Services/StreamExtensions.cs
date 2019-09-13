using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Services
{
    internal static class StreamExtensions
    {
        public static async Task<string> GetResponseStringAsync(this Stream stream, CancellationToken cancellationToken)
        {
            if (null == stream)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var text = new StringBuilder();

            using (var reader = new StreamReader(stream))
            {
                var buffer = new char[8096];

                while (true)
                {
                    var count = await reader.ReadAsync(buffer, 0, buffer.Length);

                    if (0 == count)
                    {
                        break;
                    }

                    text.Append(buffer, 0, count);
                }
            }

            return text.ToString();
        }
    }
}