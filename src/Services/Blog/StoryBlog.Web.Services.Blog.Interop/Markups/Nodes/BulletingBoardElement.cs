namespace StoryBlog.Web.Services.Blog.Interop.Markups.Nodes
{
    public abstract class BulletingBoardElement
    {
        protected BulletingBoardElement()
        {
        }

        protected static int FindLineEnd(string text, int start, int end, out int nextLineStart)
        {
            const int notfound = -1;

            var position = text.IndexOf(Terminals.Lf, start, end - start);

            if (notfound == position)
            {
                position = text.IndexOf(Terminals.Cr, start, end - start);

                if (notfound == position)
                {
                    nextLineStart = end;

                    return end;
                }
            }

            nextLineStart = position + 1;

            if (position > start && Terminals.Cr == text[position - 1])
            {
                return position - 1;
            }

            return position;
        }
    }
}