using Microsoft.AspNetCore.WebUtilities;
using StoryBlog.Web.Services.Blog.Application.Models;
using System;
using System.IO;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public static class NavigationCursorEncoder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public static bool TryParse(string str, out NavigationCursor cursor)
        {
            if (null == str)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (0 == str.Length)
            {
                cursor = null;
                return false;
            }

            var stream = new MemoryStream(Base64UrlTextEncoder.Decode(str));

            using (var reader = new BinaryReader(stream))
            {
                var id = reader.ReadInt64();
                var count = reader.ReadInt32();

                cursor = new NavigationCursor(id, count);
            }

            return true;
        }

        public static string ToEncodedString(NavigationCursor cursor)
        {
            var stream = new MemoryStream();

            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(cursor.Id);
                writer.Write(cursor.Count);
            }

            return Base64UrlTextEncoder.Encode(stream.ToArray());
        }
    }
}