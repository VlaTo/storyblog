namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Author
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public Author(string name)
        {
            Name = name;
        }
    }
}