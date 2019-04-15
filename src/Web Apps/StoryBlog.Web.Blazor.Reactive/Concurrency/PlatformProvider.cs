using System;
using System.Security.Cryptography.X509Certificates;

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

            public TService GetService<TService>(string key) 
                where TService : class
            {
                if (null == key)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                if (String.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentException("", nameof(key));
                }

                return (TService) GetService(typeof(TService), key);
            }

            public object GetService(Type serviceType)
            {
                if (null == serviceType)
                {
                    throw new ArgumentNullException(nameof(serviceType));
                }

                return GetService(serviceType);
            }

            private object GetService(Type serviceType, string key)
            {
                if (typeof(IConcurrencyAbstraction) == serviceType)
                {
                    return new ConcurrencyAbstractionImpl();
                }

                if (typeof(IScheduler) == serviceType)
                {
                    switch (key)
                    {
#if !WINDOWS && !NO_THREAD
                        case "ThreadPool":
                        {
                            return (object) ThreadPoolScheduler.Instance;
                        }
#elif WINDOWS
                        case "ThreadPool":
                        {
                            return (object) ThreadPoolScheduler.Default;
                        }
#endif
                        case "TaskPool":
                        {
                            return (object) TaskPoolScheduler.Default;
                        }

                        case "NewThread":
                        {
                            return (object) NewThreadScheduler.Default;
                        }
                    }
                }

                return null;
            }
        }
    }
}