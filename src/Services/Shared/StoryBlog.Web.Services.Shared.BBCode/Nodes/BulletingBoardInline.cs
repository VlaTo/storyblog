namespace StoryBlog.Web.Services.Shared.BBCode.Nodes
{
    /// <summary>
    /// 
    /// </summary>
    public enum BulletingBoardInlineType
    {
        /// <summary>
        /// 
        /// </summary>
        Text
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BulletingBoardInline : BulletingBoardElement
    {
        /// <summary>
        /// 
        /// </summary>
        public BulletingBoardInlineType InlineType
        {
            get;
        }

        protected BulletingBoardInline(BulletingBoardInlineType type)
        {
            InlineType = type;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class BulletingBoardText : BulletingBoardInline
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get;
        }

        private BulletingBoardText(string text)
            : base(BulletingBoardInlineType.Text)
        {
            Text = text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static BulletingBoardText Read(string text, ref int start, int end)
        {
            var position = start;
            var count = 0;

            while (position < end)
            {
                var ch = text[position];

                if ('[' == ch)
                {
                    break;
                }

                if (Terminals.Cr == ch)
                {
                    var next = position + 1;

                    if (next < end && Terminals.Lf == text[next])
                    {
                        position += 2;
                    }

                    break;
                }

                if (Terminals.Lf == ch)
                {

                }

                position++;
                count++;
            }

            if (position == start)
            {
                return null;
            }

            var block = new BulletingBoardText(text.Substring(start, count));

            start = position;

            return block;
        }
    }
}