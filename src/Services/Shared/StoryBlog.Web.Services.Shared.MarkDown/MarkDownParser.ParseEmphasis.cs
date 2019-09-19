using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.MarkDown.Elements;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    internal partial class MarkDownParser
    {
        private async Task ParseEmphasisAsync(IList<MarkDownElement> children)
        {
            var state = ParseEmphasisState.Text;
            var builder = new Lazy<StringBuilder>();

            bool IsNotDone() => ParseEmphasisState.Failed != state || ParseEmphasisState.Complete != state;

            while (IsNotDone())
            {
                var token = await tokenizer.GetNextTokenAsync().ConfigureAwait(false);

                if (null == token)
                {
                    state = ParseEmphasisState.Complete;
                    break;
                }

                if (token.TryGetTerminal(out var terminal))
                {
                    if (token.IsNewLine())
                    {
                        if (ParseEmphasisState.LeadingUnderline == state)
                        {
                            ;
                        }

                        if (ParseEmphasisState.TrailingUnderline == state)
                        {
                            ;
                        }

                        state = ParseEmphasisState.Complete;

                        break;
                    }

                    if (Char.IsWhiteSpace(terminal))
                    {
                        if (builder.IsValueCreated && 0 < builder.Value.Length)
                        {
                            builder.Value.Append(terminal);
                        }

                        continue;
                    }

                    switch (terminal)
                    {
                        case '_':
                        {
                            if (ParseEmphasisState.Text == state)
                            {
                                state = ParseEmphasisState.LeadingUnderline;
                                continue;
                            }

                            if (ParseEmphasisState.LeadingUnderline == state)
                            {
                                var bold = new MarkDownBoldElement();

                                children.Add(bold);

                                state = ParseEmphasisState.TrailingUnderline;
                                
                                continue;
                            }

                            break;
                        }

                        case '*':
                        {
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
                    if (ParseEmphasisState.LeadingUnderline == state)
                    {
                        var italic = new MarkDownItalicElement();

                        await ParseEmphasisAsync(italic.Children).ConfigureAwait(false);

                        continue;
                    }

                    if (ParseEmphasisState.TrailingUnderline == state)
                    {

                    }

                    builder.Value.Append(text);

                    continue;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private enum ParseEmphasisState
        {
            Failed = -1,
            Text,
            LeadingUnderline,
            TrailingUnderline,
            Complete
        }
    }
}