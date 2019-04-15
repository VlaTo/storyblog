using System;
using System.Threading;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// Represents an object that is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed observers.
    /// </summary>
    /// <typeparam name="T">The type of the elements processed by the subject.</typeparam>
    public sealed class Subject<T> : SubjectBase<T>
    {
        private static readonly SubjectDisposable[] EMPTY = new SubjectDisposable[0];
        private static readonly SubjectDisposable[] TERMINATED = new SubjectDisposable[0];
        private static readonly SubjectDisposable[] DISPOSED = new SubjectDisposable[0];
        private SubjectDisposable[] observers;
        private Exception exception;

        /// <summary>
        /// Creates a subject.
        /// </summary>
        public Subject()
        {
            Volatile.Write(ref observers, EMPTY);
        }

        /// <summary>
        /// Indicates whether the subject has observers subscribed to it.
        /// </summary>
        public override bool HasObservers => 0 != Volatile.Read(ref observers).Length;

        /// <summary>
        /// Indicates whether the subject has been disposed.
        /// </summary>
        public override bool IsDisposed => Volatile.Read(ref observers) == DISPOSED;

        private void ThrowDisposed() => throw new ObjectDisposedException(string.Empty);

        /// <summary>
        /// Notifies all subscribed observers about the end of the sequence.
        /// </summary>
        public override void OnCompleted()
        {
            for (;;)
            {
                var subscribers = Volatile.Read(ref observers);

                if (subscribers == DISPOSED)
                {
                    exception = null;
                    ThrowDisposed();
                    break;
                }

                if (subscribers == TERMINATED)
                {
                    break;
                }

                if (Interlocked.CompareExchange(ref observers, TERMINATED, subscribers) == subscribers)
                {
                    foreach (var observer in subscribers)
                    {
                        observer.Observer?.OnCompleted();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the specified exception.
        /// </summary>
        /// <param name="error">The exception to send to all currently subscribed observers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="error"/> is null.</exception>
        public override void OnError(Exception error)
        {
            if (null == error)
            {
                throw new ArgumentNullException(nameof(error));
            }

            for (; ; )
            {
                var subscribers = Volatile.Read(ref observers);

                if (subscribers == DISPOSED)
                {
                    exception = null;
                    ThrowDisposed();
                    break;
                }

                if (subscribers == TERMINATED)
                {
                    break;
                }

                exception = error;

                if (Interlocked.CompareExchange(ref observers, TERMINATED, subscribers) == subscribers)
                {
                    foreach (var observer in subscribers)
                    {
                        observer.Observer?.OnError(error);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Notifies all subscribed observers about the arrival of the specified element in the sequence.
        /// </summary>
        /// <param name="value">The value to send to all currently subscribed observers.</param>
        public override void OnNext(T value)
        {
            var subscribers = Volatile.Read(ref this.observers);

            if (subscribers == DISPOSED)
            {
                exception = null;
                ThrowDisposed();

                return;
            }

            foreach (var disposable in subscribers)
            {
                disposable.Observer?.OnNext(value);
            }
        }

        /// <summary>
        /// Subscribes an observer to the subject.
        /// </summary>
        /// <param name="observer">Observer to subscribe to the subject.</param>
        /// <returns>Disposable object that can be used to unsubscribe the observer from the subject.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="observer"/> is null.</exception>
        public override IDisposable Subscribe(IObserver<T> observer)
        {
            if (null == observer)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            var disposable = default(SubjectDisposable);

            for (; ; )
            {
                var subscribers = Volatile.Read(ref observers);

                if (subscribers == DISPOSED)
                {
                    exception = null;
                    ThrowDisposed();

                    break;
                }

                if (subscribers == TERMINATED)
                {
                    var ex = exception;

                    if (ex != null)
                    {
                        observer.OnError(ex);
                    }
                    else
                    {
                        observer.OnCompleted();
                    }

                    break;
                }

                if (disposable == null)
                {
                    disposable = new SubjectDisposable(this, observer);
                }

                var n = subscribers.Length;
                var b = new SubjectDisposable[n + 1];

                Array.Copy(subscribers, 0, b, 0, n);

                b[n] = disposable;

                if (Interlocked.CompareExchange(ref observers, b, subscribers) == subscribers)
                {
                    return disposable;
                }
            }

            return Disposable.Empty;
        }

        private void Unsubscribe(SubjectDisposable observer)
        {
            for (; ; )
            {
                var a = Volatile.Read(ref observers);
                var n = a.Length;

                if (n == 0)
                {
                    break;
                }

                var j = Array.IndexOf(a, observer);

                if (j < 0)
                {
                    break;
                }

                SubjectDisposable[] b;

                if (n == 1)
                {
                    b = EMPTY;
                }
                else
                {
                    b = new SubjectDisposable[n - 1];
                    Array.Copy(a, 0, b, 0, j);
                    Array.Copy(a, j + 1, b, j, n - j - 1);
                }
                if (Interlocked.CompareExchange(ref observers, b, a) == a)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class SubjectDisposable : IDisposable
        {
            private Subject<T> subject;
            private IObserver<T> observer;

            public IObserver<T> Observer => Volatile.Read(ref observer);

            public SubjectDisposable(Subject<T> subject, IObserver<T> observer)
            {
                this.subject = subject;
                Volatile.Write(ref this.observer, observer);
            }

            public void Dispose()
            {
                var subscriber = Interlocked.Exchange(ref this.observer, null);

                if (null == subscriber)
                {
                    return;
                }

                subject.Unsubscribe(this);
                subject = null;
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Subject{T}"/> class and unsubscribes all observers.
        /// </summary>
        public override void Dispose()
        {
            Interlocked.Exchange(ref observers, DISPOSED);
            exception = null;
        }
    }
}