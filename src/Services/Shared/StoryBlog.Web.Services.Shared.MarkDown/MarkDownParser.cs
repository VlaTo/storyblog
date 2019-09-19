using StoryBlog.Web.Services.Shared.MarkDown.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    internal partial class MarkDownParser : IDisposable
    {
        private MarkDownTokenizer tokenizer;
        private bool disposed;

        public MarkDownParser(MarkDownTokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public async Task<IList<MarkDownElement>> ParseAsync(MarkDownParseOptions options)
        {
            EnsureNotDisposed();

            var nodes = new List<MarkDownElement>();

            await ParseEmphasisAsync(nodes);

            /*while (true)
            {
                var token = await tokenizer.GetNextTokenAsync().ConfigureAwait(false);

                if (null == token)
                {
                    break;
                }

                if (token.TryGetTerminal(out var terminal))
                {
                    if ('#' == terminal)
                    {
                        await ParseHeading(nodes).ConfigureAwait(false);
                        continue;
                    }

                    /-if ('_' == terminal)
                    {
                        ParseEmphasisAsync()
                        continue;
                    }-/

                    break;
                }

                if (token.TryGetText(out var text))
                {
                    var line = text;

                    await ParseToLineEndAsync(tail => line += tail).ConfigureAwait(false);

                    nodes.Add(new MarkDownTextElement(line));

                    continue;
                }
            }*/

            return nodes;
        }

        private async Task ParseHeading(IList<MarkDownElement> children)
        {
            var headingLevel = 1;

            while (true)
            {
                var token = await tokenizer.GetNextTokenAsync().ConfigureAwait(false);

                if (null == token)
                {
                    break;
                }

                if (token.TryGetTerminal(out var terminal))
                {
                    switch (terminal)
                    {
                        case '#':
                        {
                            headingLevel++;
                            continue;
                        }

                        case ' ':
                        {
                            var heading = new MarkDownHeadingElement(headingLevel);

                            children.Add(heading);

                            await ParseToLineEndAsync(tail =>
                            {
                                var node = new MarkDownTextElement(tail);
                                heading.Children.Add(node);
                            }).ConfigureAwait(false);

                            return;
                        }

                        default:
                        {
                            var line = new String('#', headingLevel);

                            await ParseToLineEndAsync(tail => line += tail).ConfigureAwait(false);

                            children.Add(new MarkDownTextElement(line));

                            return;
                        }
                    }
                }

                if (token.TryGetText(out var text))
                {
                    var line = text;

                    await ParseToLineEndAsync(tail => line += tail).ConfigureAwait(false);

                    children.Add(new MarkDownTextElement(line));

                    break;
                }
            }
        }

        private async Task ParseToLineEndAsync(Action<string> callback)
        {
            var builder = new Lazy<StringBuilder>();

            while (true)
            {
                var token = await tokenizer.GetNextTokenAsync().ConfigureAwait(false);

                if (null == token)
                {
                    break;
                }

                if (token.TryGetTerminal(out var terminal))
                {
                    if ('\n' == terminal)
                    {
                        break;
                    }

                    if ('_' == terminal)
                    {

                    }

                    builder.Value.Append(terminal);

                    continue;
                }

                if (token.TryGetText(out var text))
                {
                    builder.Value.Append(text);
                    continue;
                }

                throw new Exception();
            }

            if (builder.IsValueCreated && 0 < builder.Value.Length)
            {
                //var text = new MarkDownTextElements(builder.Value.ToString());
                callback.Invoke(builder.Value.ToString());
            }
        }

        private async Task ParseSimpleAsync(IList<MarkDownElement> children)
        {
            var builder = new Lazy<StringBuilder>();

            while (true)
            {
                var token = await tokenizer.GetNextTokenAsync().ConfigureAwait(false);

                if (null == token)
                {
                    break;
                }

                if (token.TryGetTerminal(out var terminal))
                {
                    if (Char.IsWhiteSpace(terminal))
                    {
                        if (builder.IsValueCreated)
                        {

                        }

                        builder.Value.Append(terminal);

                        continue;
                    }

                    switch (terminal)
                    {
                        case '_':
                        {
                            var emphasis = new MarkDownItalicElement();

                            children.Add(emphasis);

                            await ParseEmphasisAsync(emphasis.Children).ConfigureAwait(false);

                            break;
                        }

                        case '*':
                        {
                            await ParseEmphasisAsync(children).ConfigureAwait(false);

                            break;
                        }

                        default:
                        {
                            break;
                        }
                    }

                    continue;
                }

                if (token.TryGetText(out var text))
                {
                    if (builder.IsValueCreated)
                    {
                        ;
                    }

                    builder.Value.Append(text);

                    continue;
                }
            }
        }


        private void EnsureNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    tokenizer.Dispose();
                    tokenizer = null;
                }
            }
            finally
            {
                disposed = true;
            }
        }
    }
}