using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace StoryBlog.Web.Services.Identity.API.Configuration.Certificates
{
    internal static class Certificate
    {
        public static X509Certificate2 Get()
        {
            var assembly = typeof(Certificate).Assembly;
            var names = assembly.GetManifestResourceNames();
            var resource = GetCertificateResourceName(names);

            using (var stream = assembly.GetManifestResourceStream(resource))
            {
                return new X509Certificate2(GetBytesFromStream(stream));
            }
        }

        private static string GetCertificateResourceName(IEnumerable<string> names)
        {
            const string extension = ".pfx";
            var ns = typeof(Certificate).Namespace;
            return names.First(name => name.StartsWith(ns) && name.EndsWith(extension));
        }

        private static byte[] GetBytesFromStream(Stream stream)
        {
            var buffer = new Span<byte>(new byte[4 * 1024]);

            using (var memory = new MemoryStream())
            {
                int count;

                while ((count = stream.Read(buffer)) > 0)
                {
                    memory.Write(buffer.Slice(0, count));
                }

                return memory.ToArray();
            }
        }
    }
}