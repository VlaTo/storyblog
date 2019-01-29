namespace StoryBlog.Web.Services.Shared.Data.Csv.Tokens
{
    /// <summary>
    /// 
    /// </summary>
    internal enum CsvTokenType
    {
        Unknown = -1,
        End,
        Terminal,
        String
    }

    /// <summary>
    /// 
    /// </summary>
    internal class CsvToken
    {
        public static readonly CsvToken End;
        public static readonly CsvToken Unknown;

        public CsvTokenType TokenType
        {
            get;
        }

        public static CsvStringToken String(string text)
        {
            return new CsvStringToken(text);
        }

        public static CsvTerminalToken Terminal(char term)
        {
            return new CsvTerminalToken(term);
        }

        protected CsvToken(CsvTokenType tokenType)
        {
            TokenType = tokenType;
        }

        static CsvToken()
        {
            End = new CsvToken(CsvTokenType.End);
            Unknown = new CsvToken(CsvTokenType.Unknown);
        }
    }
}