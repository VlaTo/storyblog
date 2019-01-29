namespace StoryBlog.Web.Services.Shared.Data.Csv.Tokens
{
    internal class CsvStringToken : CsvToken
    {
        public string Text
        {
            get;
        }

        public CsvStringToken(string text)
            : base(CsvTokenType.String)
        {
            Text = text;
        }
    }
}