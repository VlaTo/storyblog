using System;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    internal static class TokenExtensions
    {
        internal static bool TryGetTerm(this BBCodeMarkup.IToken token, out char term)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token is BBCodeMarkup.TermToken termToken)
            {
                term = termToken.Char;
                return true;
            }

            term = '\0';

            return false;
        }

        internal static bool IsTerm(this BBCodeMarkup.IToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token is BBCodeMarkup.TermToken)
            {
                return true;
            }

            return false;
        }

        internal static bool TryGetText(this BBCodeMarkup.IToken token, out string text)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (token is BBCodeMarkup.TextToken textToken)
            {
                text = textToken.Text;
                return true;
            }

            text = null;

            return false;
        }
    }
}