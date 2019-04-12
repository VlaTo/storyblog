namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// (Infrastructure) Concurrency abstraction layer.
    /// </summary>
    internal static class ConcurrencyAbstraction
    {
        /// <summary>
        /// Gets the current CAL. If no CAL has been set yet, it will be initialized to the default.
        /// </summary>
        public static IConcurrencyAbstraction Current
        {
            get;
        }

        static ConcurrencyAbstraction()
        {
            Current = PlatformProvider.Current.GetService<IConcurrencyAbstraction>();
        }
    }
}