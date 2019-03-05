using StoryBlog.Web.Services.Shared.Data.Csv.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    internal class CsvParser
    {
        private readonly CsvTokenizer tokenizer;

        public CsvParser(CsvTokenizer tokenizer)
        {
            if (null == tokenizer)
            {
                throw new ArgumentNullException(nameof(tokenizer));
            }

            this.tokenizer = tokenizer;
        }

        public void Parse(CsvDocument document, CsvParsingOptions options)
        {
            try
            {
                ParseInternal(document, options);
            }
            catch (Exception exception)
            {
                throw new CsvParsingException(exception.Message, exception);
            }
        }

        internal void ParseInternal(CsvDocument document, CsvParsingOptions options)
        {
            var hasHeader = false;
            var addRow = new Action<ICollection<string>>(fields =>
            {
                var row = document.CreateRow();

                fields.ForEach(text => row.Fields.Add(document.CreateField(row, text)));

                document.Rows.Add(row);
            });

            var state = ParserState.Begin;

            while (ParserState.Done != state)
            {
                switch (state)
                {
                    case ParserState.Begin:
                    {
                        state = ParseLine(fields =>
                        {
                            if (options.HasHeader && false == hasHeader)
                            {
                                hasHeader = true;
                                fields.ForEach(field => document.Names.Add(field));

                                return;
                            }

                            addRow.Invoke(fields);

                        });

                        break;
                    }

                    case ParserState.NewLine:
                    {
                        state = ParseLine(addRow);

                        break;
                    }

                    default:
                    {
                        throw new CsvParsingException();
                    }
                }
            }
        }

        private ParserState ParseLine(Action<ICollection<string>> callback)
        {
            var state = ParserState.BeginLine;
            Collection<string> fields = null;

            while (true)
            {
                switch (state)
                {
                    case ParserState.BeginLine:
                    {
                        state = ParserState.NextField;
                        fields = new Collection<string>();

                        break;
                    }

                    case ParserState.NextField:
                    {
                        var collection = fields;

                        state = ParseField(field =>
                        {
                            if (null == collection)
                            {
                                throw new InvalidOperationException();
                            }

                            collection.Add(field);

                        });

                        break;
                    }

                    case ParserState.Field:
                    {
                        state = ParseComma();
                        break;
                    }

                    case ParserState.EndLine:
                    {
                        if (null == fields)
                        {
                            throw new InvalidOperationException();
                        }

                        callback.Invoke(fields);
                        state = ParserState.BeginLine;

                        break;
                    }

                    case ParserState.Done:
                    {
                        if (null == fields)
                        {
                            throw new InvalidOperationException();
                        }

                        callback.Invoke(fields);

                        return ParserState.Done;
                    }

                    default:
                    {
                        return ParserState.Failed;
                    }
                }
            }
        }

        private ParserState ParseComma()
        {
            CsvToken token;

            while (true)
            {
                token = tokenizer.GetToken();

                if (token.IsWhitespace())
                {
                    continue;
                }

                break;
            }

            var lineFeed = false;

            while (true)
            {
                if (token.IsEnd())
                {
                    return ParserState.Done;
                }

                if (token.IsComma())
                {
                    return ParserState.NextField;
                }

                if (token.IsTerminal(CsvTerminals.LineFeed))
                {
                    lineFeed = true;
                    token = tokenizer.GetToken();

                    continue;
                }

                if (token.IsTerminal(CsvTerminals.NewLine))
                {
                    if (lineFeed)
                    {

                    }

                    //return ParserState.NewLine;
                    return ParserState.EndLine;
                }

                break;
            }

            return ParserState.Failed;
        }

        /*private async Task<ParserState> ParseBeginLineAsync(Action<string> callback)
        {
            CsvToken token;

            while (true)
            {
                token = await tokenizer.GetTokenAsync().ConfigureAwait(false);

                if (token.IsWhitespace())
                {
                    continue;
                }

                break;
            }

            if (token.IsEnd())
            {
                return ParserState.Done;
            }

            if (token.IsTerminal(CsvTerminals.Comma))
            {
                return ParserState.NextField;
            }

            if (token.IsTerminal(CsvTerminals.NewLine))
            {
                return ParserState.NewLine;
            }

            if (token.IsString(out var text))
            {
                callback.Invoke(text);
                return ParserState.Field;
            }

            return ParserState.Failed;
        }*/

        private ParserState ParseField(Action<string> callback)
        {
            CsvToken token;

            while (true)
            {
                token = tokenizer.GetToken();

                if (token.IsWhitespace())
                {
                    continue;
                }

                break;
            }

            if (token.IsDoubleQuote())
            {
                return ParseQuotedField(callback);
            }

            ParserState state;
            var lineFeed = false;
            var text = new StringBuilder();

            while (true)
            {
                if (token.IsEnd())
                {
                    state = ParserState.Done;
                    break;
                }

                if (token.IsComma())
                {
                    state = ParserState.NextField;
                    break;
                }

                if (token.IsTerminal(CsvTerminals.LineFeed))
                {
                    lineFeed = true;
                    token = tokenizer.GetToken();

                    continue;
                }

                if (token.IsNewLine())
                {
                    if (lineFeed)
                    {

                    }

                    state = ParserState.EndLine;

                    break;
                }

                if (token.IsString(out var str))
                {
                    text.Append(str);
                }
                else if(token.IsTerminal())
                {
                    var term = ((CsvTerminalToken) token).Term;
                    text.Append(term);
                }
                else
                {
                    state = ParserState.Failed;
                    break;
                }

                token = tokenizer.GetToken();
            }

            if (0 < text.Length)
            {
                callback.Invoke(text.ToString());
            }
            else if (ParserState.NextField == state || ParserState.NewLine == state)
            {
                callback.Invoke(null);
            }

            return state;
        }

        private ParserState ParseQuotedField(Action<string> callback)
        {
            var field = new StringBuilder();
            var done = false;

            while (false == done)
            {
                var token = tokenizer.GetToken();

                if (token.IsEnd())
                {
                    break;
                }

                if (token.IsTerminal())
                {
                    var term = ((CsvTerminalToken) token).Term;

                    switch (term)
                    {
                        case CsvTerminals.DoubleQuote:
                        {
                            done = true;
                            break;
                        }

                        default:
                        {
                            field.Append(term);
                            break;
                        }
                    }
                }

                if (token.IsString(out var text))
                {
                    field.Append(text);
                }
            }

            if (0 < field.Length)
            {
                callback.Invoke(field.ToString());

                return ParserState.Field;
            }

            return ParserState.Failed;
        }

        /// <summary>
        /// 
        /// </summary>
        internal enum ParserState
        {
            Failed = -1,
            Begin,
            Done,
            Field,
            NextField,
            NewLine,
            BeginLine,
            EndLine
        }
    }
}