using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    public partial class BBCodeMarkup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static MarkupNode Parse(string text)
        {
            var state = ParseTextState.Unknown;
            var collection = new Collection<MarkupNode>();

            using (var tokenizer = new Tokenizer(new StringReader(text)))
            {
                while (IsNotDone(state))
                {
                    switch (state)
                    {
                        case ParseTextState.Unknown:
                        {
                            state = ParseText(tokenizer, node =>
                            {
                                if (0 < collection.Count)
                                {
                                    var last = collection[collection.Count - 1];
                                    
                                    if (last is TagNode tag)
                                    {
                                        tag.Inlines.Add(node);
                                        return;
                                    }

                                    throw new Exception();
                                }

                                collection.Add(node);
                            });

                            break;
                        }

                        /*case ParseTextState.BracketTerm:
                        {
                            var token = tokenizer.NextToken();

                            if (null == token)
                            {
                                break;
                            }

                            if (token.TryGetTerm(out var term))
                            {
                                if ('[' == term)
                                {
                                    buffer.Append(term);
                                    state = ParseTextState.Text;

                                    break;
                                }

                                if (0 < buffer.Length)
                                {
                                    collection.Add(new TextNode(buffer.ToString()));
                                    buffer.Clear();
                                }

                                if ('\\' == term)
                                {
                                    state = ParseTextState.SlashTerm;
                                }
                            }

                            break;
                        }*/

                        /*case ParseTextState.Text:
                        {
                            var token = tokenizer.NextToken();

                            if (null == token)
                            {
                                state = ParseTextState.Done;
                                break;
                            }

                            if (token.TryGetTerm(out var term))
                            {
                                if ('[' == term)
                                {
                                    state = ParseTextState.BracketTerm;
                                    break;
                                }

                                buffer.Append(term);

                                break;
                            }

                            if (token.TryGetText(out var content))
                            {
                                buffer.Append(content);
                                break;
                            }

                            state = ParseTextState.Failed;

                            break;
                        }*/

                        case ParseTextState.Failed:
                        {
                            throw new Exception();
                        }
                    }
                }

                /*if (0 < buffer.Length)
                {
                    collection.Add(new TextNode(buffer.ToString()));
                    buffer.Clear();
                }*/
            }

            if (0 == collection.Count)
            {
                return null;
            }

            if (1 < collection.Count)
            {
                throw new Exception();
            }

            return collection[0];
        }

        private static ParseTextState ParseText(Tokenizer tokenizer, Action<MarkupNode> callback)
        {
            var builder = new StringBuilder();
            var state = ParseTextState.Text;

            void ReportTextNode()
            {
                if (0 == builder.Length)
                {
                    return;
                }

                var node = new TextNode(builder.ToString());

                builder.Clear();
                callback.Invoke(node);
            }

            while (IsNotDone(state))
            {
                var token = tokenizer.NextToken();

                if (null == token)
                {
                    ReportTextNode();
                    state = ParseTextState.Done;
                    continue;
                }

                if (token.TryGetTerm(out var term))
                {
                    if ('[' == term)
                    {
                        ReportTextNode();

                        state = ParseTag(tokenizer);

                        continue;
                    }

                    builder.Append(term.ToString());

                    continue;
                }

                if (token.TryGetText(out var content))
                {
                    builder.Append(content);
                    continue;
                }

                return ParseTextState.Failed;
            }

            /*if ((ParseTextState.Done == state || ParseTextState.Text == state) && 0 < builder.Length)
            {

                callback.Invoke(builder.ToString());
            }*/

            return state;
        }

        private static ParseTextState ParseTag(Tokenizer tokenizer)
        {
            var state = ParseOpenTag(tokenizer, node => { });

            if (ParseTextState.TagOpen == state)
            {
                state = ParseText(tokenizer, node1 => { });

                if (ParseTextState.Close == state || ParseTextState.Text == state)
                {
                    state = ParseCloseTag(tokenizer, name => { });
                }
            }

            return state;
        }

        private static ParseTextState ParseOpenTag(Tokenizer tokenizer, Action<TagNode> callback)
        {
            var token = tokenizer.NextToken();

            if (null == token)
            {
                return ParseTextState.Failed;
            }

            if (token.TryGetTerm(out var term))
            {
                if ('[' == term)
                {
                    return ParseTextState.SlashTerm;
                }

                return ParseTextState.Failed;
            }

            if (false == token.TryGetText(out var name))
            {
                return ParseTextState.Failed;
            }

            token = tokenizer.NextToken();

            if (token.TryGetTerm(out term))
            {
                string value = null;

                if ('=' == term)
                {
                    token = tokenizer.NextToken();

                    if (token.TryGetText(out value))
                    {

                        token = tokenizer.NextToken();

                        if (false == token.TryGetTerm(out term))
                        {
                            return ParseTextState.Failed;
                        }
                    }
                }

                if (']' != term)
                {
                    return ParseTextState.Failed;
                }

                var node = new TagNode(name, value);
                callback.Invoke(node);

                return ParseTextState.TagOpen;
            }

            return ParseTextState.Failed;
        }

        private static ParseTextState ParseCloseTag(Tokenizer tokenizer, Action<string> callback)
        {
            var token = tokenizer.NextToken();

            if (null == token)
            {
                return ParseTextState.Failed;
            }

            if (token.TryGetTerm(out var term))
            {
                if ('[' == term)
                {
                    return ParseTextState.SlashTerm;
                }

                return ParseTextState.Failed;
            }

            if (false == token.TryGetText(out var name))
            {
                return ParseTextState.Failed;
            }

            token = tokenizer.NextToken();

            if (token.TryGetTerm(out term))
            {
                if (']' != term)
                {
                    return ParseTextState.Failed;
                }

                callback.Invoke(name);

                return ParseTextState.Close;
            }

            return ParseTextState.Failed;
        }

        private static ParseTextState ParseTagName(Tokenizer tokenizer, out string name)
        {
            var token = tokenizer.NextToken();

            if (null == token)
            {
                name = null;
                return ParseTextState.EOS;
            }

            if (token.TryGetText(out name))
            {
                return ParseTextState.Name;
            }

            return ParseTextState.Failed;
        }

        /*
                private static TagToken ReadTag(StringReader reader)
                {
                    var state = OpenTagReadState.Unknown;
                    var name = new StringBuilder();

                    var value = reader.ReadNext();

                    while (true)
                    {
                        switch (state)
                        {
                            case OpenTagReadState.Unknown:
                            {
                                if (-1 == value)
                                {
                                    return null;
                                }

                                state = OpenTagReadState.Opening;

                                break;
                            }

                            case OpenTagReadState.Opening:
                            {
                                var ch = (char) value;

                                if ('/' == ch)
                                {
                                    state = OpenTagReadState.ClosingName;
                                }
                                else if ('[' == ch)
                                {
                                    state = OpenTagReadState.SquareBracket;
                                }
                                else if (Char.IsLetter(ch))
                                {
                                    name.Append(ch);
                                    state = OpenTagReadState.Name;

                                    continue;
                                }

                                break;
                            }

                            case OpenTagReadState.Name:
                            {
                                value = reader.ReadNext();

                                if (-1 == value)
                                {
                                    return null;
                                }

                                var ch = (char) value;

                                if (Char.IsLetterOrDigit(ch) || '-' == ch)
                                {
                                    name.Append(ch);
                                }
                                else if (']' == ch)
                                {
                                    return new Tag(TagMode.Opening, name.ToString());
                                }

                                break;
                            }

                            case OpenTagReadState.ClosingName:
                            {
                                value = reader.ReadNext();

                                if (-1 == value)
                                {
                                    return null;
                                }

                                var ch = (char) value;

                                if (Char.IsLetterOrDigit(ch) || '-' == ch)
                                {
                                    name.Append(ch);
                                }
                                else if (']' == ch)
                                {
                                    return new Tag(TagMode.Closing, name.ToString());
                                }

                                break;
                            }
                        }
                    }

                    return null;
                }
        */

        private static bool IsNotDone(ParseTextState state) =>
            ParseTextState.Done != state && ParseTextState.Failed != state;

        /// <summary>
        /// 
        /// </summary>
        private enum ParseTextState
        {
            Failed = -1,
            Unknown,
            Done,
            TagOpen,
            TagName,
            Opening,
            Name,
            BracketTerm,
            ClosingName,
            Close,
            Text,
            SlashTerm,
            EOS
        }

        /// <summary>
        /// 
        /// </summary>
        internal interface IToken
        {
        }

        /// <summary>
        /// 
        /// </summary>
        internal class TextToken : IToken
        {
            public string Text { get; }

            public TextToken(string text)
            {
                Text = text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal class TermToken : IToken
        {
            public char Char { get; }

            public TermToken(char ch)
            {
                Char = ch;
            }
        }
    }
}