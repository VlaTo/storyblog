using System;
using System.Threading;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// This class fuses logic from ObserverBase, AnonymousObserver, and SafeObserver into one class. When an observer
    /// needs to be safeguarded, an instance of this type can be created by SafeObserver.Create when it detects its
    /// input is an AnonymousObserver, which is commonly used by end users when using the Subscribe extension methods
    /// that accept delegates for the On* handlers. By doing the fusion, we make the call stack depth shorter which
    /// helps debugging and some performance.
    /// </summary>
    internal sealed class AnonymousSafeObserver<T> : SafeObserver<T>
    {
        private readonly Action<T> _onNext;
        private readonly Action<Exception> _onError;
        private readonly Action _onCompleted;

        private int isStopped;

        public AnonymousSafeObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            _onNext = onNext;
            _onError = onError;
            _onCompleted = onCompleted;
        }

        public override void OnNext(T value)
        {
            if (isStopped == 0)
            {
                var __noError = false;
                try
                {
                    _onNext(value);
                    __noError = true;
                }
                finally
                {
                    if (!__noError)
                    {
                        Dispose();
                    }
                }
            }
        }

        public override void OnError(Exception error)
        {
            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                using (this)
                {
                    _onError(error);
                }
            }
        }

        public override void OnCompleted()
        {
            if (Interlocked.Exchange(ref isStopped, 1) == 0)
            {
                using (this)
                {
                    _onCompleted();
                }
            }
        }
    }
}