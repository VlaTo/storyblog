using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.MarkDown.Elements;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    public sealed class MarkDownDocument : MarkDownNode
    {
        private MarkDownDocument(IList<MarkDownElement> children)
            : base(children)
        {
        }

        public static async Task<MarkDownDocument> ParseAsync(string text, MarkDownParseOptions options = default)
        {
            if (null == text)
            {
                throw new ArgumentNullException(nameof(text));
            }

            using (var reader = new StringReader(text))
            {
                return await ParseInternalAsync(reader, options).ConfigureAwait(false);
            }
        }

        public static async Task<MarkDownDocument> ParseAsync(byte[] data, MarkDownParseOptions options = default)
        {
            if (null == data)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var stream = new MemoryStream(data);

            using (var reader = new StreamReader(stream))
            {
                return await ParseInternalAsync(reader, options).ConfigureAwait(false);
            }
        }

        private static async Task<MarkDownDocument> ParseInternalAsync(TextReader reader, MarkDownParseOptions options)
        {
            var tokenizer = new MarkDownTokenizer(reader);

            using (var parser = new MarkDownParser(tokenizer))
            {
                var elements = await parser.ParseAsync(options).ConfigureAwait(false);

                if (null == elements)
                {
                    ;
                }

                return new MarkDownDocument(elements);
            }
        }
    }
}
