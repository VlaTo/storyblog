namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public enum NavigationCursorDirection : sbyte
    {
        /// <summary>
        /// 
        /// </summary>
        Backward = -1,

        /// <summary>
        /// 
        /// </summary>
        Forward = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class NavigationCursor
    {
        /// <summary>
        /// 
        /// </summary>
        public NavigationCursorDirection Direction
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="id"></param>
        /// <param name="count"></param>
        public NavigationCursor(NavigationCursorDirection direction, long id, int count)
        {
            Direction = direction;
            Id = id;
            Count = count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static NavigationCursor Backward(long id, int count)
        {
            return new NavigationCursor(NavigationCursorDirection.Backward, id, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static NavigationCursor Forward(long id, int count)
        {
            return new NavigationCursor(NavigationCursorDirection.Forward, id, count);
        }
    }
}