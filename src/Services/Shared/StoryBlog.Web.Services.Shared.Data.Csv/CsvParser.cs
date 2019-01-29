using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.Data.Csv.Tokens;

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

        public async Task ParserAsync(CsvDocument document, CsvParsingOptions options)
        {
            try
            {
                await ParseInternalAsync(document, options).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new CsvParsingException(exception.Message, exception);
            }
        }

        internal async Task ParseInternalAsync(CsvDocument document, CsvParsingOptions options)
        {
            var addRow = new Action<ICollection<string>>(fields =>
            {
                var row = new CsvRow();

                fields.ForEach(text => row.Fields.Add(document.CreateField(text)));

                document.Rows.Add(row);
            });

            var state = ParserState.Begin;

            while (ParserState.Done != state)
            {
                switch (state)
                {
                    case ParserState.Begin:
                    {
                        state = await ParseLineAsync(fields =>
                        {
                            if (options.HasHeader)
                            {
                                fields.ForEach(field => document.Names.Add(field));
                                return;
                            }

                            addRow.Invoke(fields);

                        }).ConfigureAwait(false);

                        break;
                    }

                    case ParserState.NewLine:
                    {
                        state = await ParseLineAsync(addRow).ConfigureAwait(false);

                        break;
                    }

                    default:
                    {
                        throw new CsvParsingException();
                    }
                }
            }
        }

        private async Task<ParserState> ParseLineAsync(Action<ICollection<string>> callback)
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

                        state = await ParseFieldAsync(field =>
                        {
                            if (null == collection)
                            {
                                throw new InvalidOperationException();
                            }

                            collection.Add(field);

                        }).ConfigureAwait(false);

                        break;
                    }

                    case ParserState.Field:
                    {
                        state = await ParseCommaAsync().ConfigureAwait(false);
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

        private async Task<ParserState> ParseCommaAsync()
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

            if (token.IsComma())
            {
                return ParserState.NextField;
            }

            if (token.IsTerminal(CsvTerminals.NewLine))
            {
                return ParserState.NewLine;
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

        private async Task<ParserState> ParseFieldAsync(Action<string> callback)
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

            if (token.IsDoubleQuote())
            {
                return await ParseQuotedFieldAsync(callback);
            }

            ParserState state;
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

                token = await tokenizer.GetTokenAsync().ConfigureAwait(false);
            }

            if (0 < text.Length)
            {
                callback.Invoke(text.ToString());
            }

            return state;
        }

        private async Task<ParserState> ParseQuotedFieldAsync(Action<string> callback)
        {
            var field = new StringBuilder();

            while (true)
            {
                var token = await tokenizer.GetTokenAsync().ConfigureAwait(false);

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