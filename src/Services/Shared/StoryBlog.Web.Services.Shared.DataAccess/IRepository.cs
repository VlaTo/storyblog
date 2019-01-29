using System;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.DataAccess
{
    /// <summary>
    /// Repository abstraction interface.
    /// </summary>
    public interface IRepository : IDisposable
    {
        Task CommitAsync();
    }
}