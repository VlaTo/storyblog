using Microsoft.AspNetCore.WebUtilities;
using StoryBlog.Web.Services.Blog.Application.Models;
using System;
using System.IO;
using System.Text.Encodings.Web;
using IdentityModel;
using Microsoft.AspNetCore.Mvc.Routing;

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

            if (0 < str.Length)
            {
                var stream = new MemoryStream(Base64UrlTextEncoder.Decode(str));

                if (TryRead(stream, out var dir, out var id, out var count))
                {
                    var direction = Enum.ToObject(typeof(NavigationCursorDirection), dir);

                    cursor = new NavigationCursor((NavigationCursorDirection) direction, id, count);

                    return true;
                }
            }

            cursor = null;

            return false;
        }

        public static string ToEncodedString(NavigationCursor cursor)
        {
            var stream = new MemoryStream();

            Write(stream, Convert.ToSByte(cursor.Direction), cursor.Id, cursor.Count);

            return Base64UrlTextEncoder.Encode(stream.ToArray());
        }

        private static bool TryRead(Stream stream, out sbyte dir, out long id, out int count)
        {
            using (var reader = new BinaryReader(stream))
            {
                dir = reader.ReadSByte();
                id = reader.ReadInt64();
                count = reader.ReadInt32();
            }

            return true;
        }

        private static void Write(Stream stream, sbyte dir, long id, int count)
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(dir);
                writer.Write(id);
                writer.Write(count);
            }
        }
    }
}