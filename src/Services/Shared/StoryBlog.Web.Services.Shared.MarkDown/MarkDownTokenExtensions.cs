using System;

namespace StoryBlog.Web.Services.Shared.MarkDown
{
    internal static class MarkDownTokenExtensions
    {
        public static bool TryGetText(this MarkDownToken token, out string text)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (MarkDownTokenType.Text == token.TokenType)
            {
                text = ((MarkDownText) token).Text;
                return true;
            }

            text = null;

            return false;
        }
        
        public static bool TryGetTerminal(this MarkDownToken token, out char terminal)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (MarkDownTokenType.Terminal == token.TokenType)
            {
                terminal = ((MarkDownTerminal) token).Terminal;
                return true;
            }

            terminal = '\0';

            return false;
        }

        public static bool IsNewLine(this MarkDownToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (MarkDownTokenType.Terminal == token.TokenType)
            {
                var terminal = ((MarkDownTerminal) token).Terminal;

                if ('\r' == terminal || '\n' == terminal)
                {
                    return true;
                }
            }

            return false;
        }
    }
}