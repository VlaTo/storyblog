namespace StoryBlog.Web.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IIdManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        string GenerateId(string prefix);
    }
}