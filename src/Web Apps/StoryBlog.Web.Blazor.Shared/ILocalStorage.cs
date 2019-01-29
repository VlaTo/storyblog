/*
 *
 */

using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILocalStorage
    {
        /// <summary>
        /// 
        /// </summary>
        Task<int> GetCountAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        Task SetItemAsync(string name, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<TValue> GetItemAsync<TValue>(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        Task RemoveItemAsync(string name);

        /// <summary>
        /// 
        /// </summary>
        Task ClearAsync();
    }
}