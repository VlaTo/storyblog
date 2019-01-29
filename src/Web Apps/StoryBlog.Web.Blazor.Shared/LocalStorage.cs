/*
 *
 */

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace StoryBlog.Web.Blazor.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalStorage : ILocalStorage
    {
        private const string Prefix = "LocalStorage.";

        /// <inheritdoc />
        public Task<int> GetCountAsync() => JSRuntime.Current.InvokeAsync<int>(Prefix + "Count");

        /// <inheritdoc />
        public Task SetItemAsync(string name, object value)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var json = Json.Serialize(value);

            return JSRuntime.Current.InvokeAsync<bool>(Prefix + "SetItem", name, json);
        }

        /// <inheritdoc />
        public async Task<TValue> GetItemAsync<TValue>(string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var json = await JSRuntime.Current.InvokeAsync<string>(Prefix + "GetItem", name);

            if (String.IsNullOrEmpty(json))
            {
                return default(TValue);
            }

            return Json.Deserialize<TValue>(json);
        }

        /// <inheritdoc />
        public Task RemoveItemAsync(string name)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            return JSRuntime.Current.InvokeAsync<bool>(Prefix + "RemoveItem", name);
        }

        /// <inheritdoc />
        public Task ClearAsync()
        {
            return JSRuntime.Current.InvokeAsync<bool>(Prefix + "Clear");
        }
    }
}