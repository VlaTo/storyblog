﻿using System;

namespace StoryBlog.Web.Blazor.Reactive
{
    internal abstract class SafeObserver<TSource> : ISafeObserver<TSource>
    {
        private sealed class WrappingSafeObserver : SafeObserver<TSource>
        {
            private readonly IObserver<TSource> _observer;

            public WrappingSafeObserver(IObserver<TSource> observer)
            {
                _observer = observer;
            }

            public override void OnNext(TSource value)
            {
                var __noError = false;
                try
                {
                    _observer.OnNext(value);
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

            public override void OnError(Exception error)
            {
                using (this)
                {
                    _observer.OnError(error);
                }
            }

            public override void OnCompleted()
            {
                using (this)
                {
                    _observer.OnCompleted();
                }
            }
        }

        public static ISafeObserver<TSource> Wrap(IObserver<TSource> observer)
        {
            if (observer is AnonymousObserver<TSource> a)
            {
                return a.MakeSafe();
            }
            else
            {
                return new WrappingSafeObserver(observer);
            }
        }

        private IDisposable _disposable;

        public abstract void OnNext(TSource value);

        public abstract void OnError(Exception error);

        public abstract void OnCompleted();

        public void SetResource(IDisposable resource)
        {
            Disposable.SetSingle(ref _disposable, resource);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Disposable.TryDispose(ref _disposable);
            }
        }
    }
}