namespace StoryBlog.Web.Services.Shared.Data.Csv.Tokens
{
    internal class CsvTerminalToken : CsvToken
    {
        public char Term
        {
            get;
        }

        public CsvTerminalToken(char term)
            : base(CsvTokenType.Terminal)
        {
            Term = term;
        }
    }
}