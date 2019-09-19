namespace StoryBlog.Web.Services.Shared.MarkDown
{
    /// <summary>
    /// 
    /// </summary>
    internal enum MarkDownTokenType
    {
        Terminal,
        Text
    }

    /// <summary>
    /// 
    /// </summary>
    internal abstract class MarkDownToken
    {
        public abstract MarkDownTokenType TokenType
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal sealed class MarkDownText : MarkDownToken
    {
        public override MarkDownTokenType TokenType { get; } = MarkDownTokenType.Text;

        public string Text
        {
            get;
        }

        public MarkDownText(string text)
        {
            Text = text;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal sealed class MarkDownTerminal : MarkDownToken
    {
        public override MarkDownTokenType TokenType { get; } = MarkDownTokenType.Terminal;

        public char Terminal
        {
            get;
        }

        public MarkDownTerminal(char terminal)
        {
            Terminal = terminal;
        }
    }
}