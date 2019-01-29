using System;

namespace StoryBlog.Web.Services.Shared.Data.Csv.Tokens
{
    internal static class CsvTokenExtensions
    {
        public static bool IsEnd(this CsvToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return CsvTokenType.End == token.TokenType;
        }

        public static bool IsWhitespace(this CsvToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (CsvTokenType.Terminal != token.TokenType)
            {
                return false;
            }

            var term = ((CsvTerminalToken)token).Term;

            return CsvTerminals.Whitespace == term
                   || CsvTerminals.Tab == term;
        }

        public static bool IsDoubleQuote(this CsvToken token)
        {
            return CheckTerminal(token, CsvTerminals.DoubleQuote);
        }

        public static bool IsComma(this CsvToken token)
        {
            return CheckTerminal(token, CsvTerminals.Comma);
        }

        public static bool IsString(this CsvToken token, out string text)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            var match = CsvTokenType.String == token.TokenType;

            text = match ? ((CsvStringToken) token).Text : null;

            return match;
        }

        public static bool IsTerminal(this CsvToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return CsvTokenType.Terminal == token.TokenType;
        }

        public static bool IsTerminal(this CsvToken token, char term)
        {
            return CheckTerminal(token, term);
        }

        private static bool CheckTerminal(this CsvToken token, char term)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (CsvTokenType.Terminal != token.TokenType)
            {
                return false;
            }

            return term == ((CsvTerminalToken) token).Term;
        }
    }
}