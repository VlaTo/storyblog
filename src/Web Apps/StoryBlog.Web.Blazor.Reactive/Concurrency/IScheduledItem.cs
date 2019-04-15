namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// Represents a work item that has been scheduled.
    /// </summary>
    /// <typeparam name="TAbsolute">Absolute time representation type.</typeparam>
    public interface IScheduledItem<out TAbsolute>
    {
        /// <summary>
        /// Gets the absolute time at which the item is due for invocation.
        /// </summary>
        TAbsolute DueTime
        {
            get;
        }

        /// <summary>
        /// Invokes the work item.
        /// </summary>
        void Invoke();
    }
}