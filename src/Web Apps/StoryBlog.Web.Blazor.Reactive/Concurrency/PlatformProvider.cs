using System;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    internal interface IPlatformProvider : IServiceProvider
    {
        TService GetService<TService>(string key = null) where TService : class;
    }

    internal static class PlatformProvider
    {
        public static IPlatformProvider Current
        {
            get;
        }

        static PlatformProvider()
        {
            Current = new CurrentPlatformProvider();
        }

        private class CurrentPlatformProvider : IPlatformProvider
        {
            public TService GetService<TService>() where TService : class => (TService)GetService(typeof(TService));

            public object GetService(Type serviceType)
            {
                if (null == serviceType)
                {
                    throw new ArgumentNullException(nameof(serviceType));
                }

                if (typeof(IConcurrencyAbstraction) == serviceType)
                {
                    return new ConcurrencyAbstractionLayerImpl();
                }

                return null;
            }
        }
    }
}
