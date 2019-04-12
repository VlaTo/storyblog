using System;
using System.Threading;

namespace StoryBlog.Web.Blazor.Reactive
{
    internal enum TrySetSingleResult
    {
        Success,
        AlreadyAssigned,
        Disposed
    }

    /// <summary>
    /// Provides a set of static methods for creating <see cref="IDisposable"/> objects.
    /// </summary>
    public static class Disposable
    {
        /// <summary>
        /// Represents a disposable that does nothing on disposal.
        /// </summary>
        private sealed class EmptyDisposable : IDisposable
        {
            /// <summary>
            /// Singleton default disposable.
            /// </summary>
            public static readonly EmptyDisposable Instance = new EmptyDisposable();

            private EmptyDisposable()
            {
            }

            /// <summary>
            /// Does nothing.
            /// </summary>
            public void Dispose()
            {
                // no op
            }
        }

        /// <summary>
        /// Gets the disposable that does nothing when disposed.
        /// </summary>
        public static IDisposable Empty => EmptyDisposable.Instance;

        /// <summary>
        /// Creates a disposable object that invokes the specified action when disposed.
        /// </summary>
        /// <param name="dispose">Action to run during the first call to <see cref="IDisposable.Dispose"/>. The action is guaranteed to be run at most once.</param>
        /// <returns>The disposable object that runs the given action upon disposal.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is <c>null</c>.</exception>
        public static IDisposable Create(Action dispose)
        {
            if (null == dispose)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            return new AnonymousDisposable(dispose);
        }

        /// <summary>
        /// Creates a disposable object that invokes the specified action when disposed.
        /// </summary>
        /// <param name="state">The state to be passed to the action.</param>
        /// <param name="dispose">Action to run during the first call to <see cref="IDisposable.Dispose"/>. The action is guaranteed to be run at most once.</param>
        /// <returns>The disposable object that runs the given action upon disposal.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dispose"/> is <c>null</c>.</exception>
        public static IDisposable Create<TState>(TState state, Action<TState> dispose)
        {
            if (null == dispose)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            return new AnonymousDisposable<TState>(state, dispose);
        }

        /// <summary>
        /// Gets the value stored in <paramref name="disposable" /> or a null if
        /// <paramref name="disposable" /> was already disposed.
        /// </summary>
        internal static IDisposable GetValue(ref IDisposable disposable)
        {
            var current = Volatile.Read(ref disposable);

            return BooleanDisposable.True == current
                ? null
                : current;
        }

        /// <summary>
        /// Gets the value stored in <paramref name="disposable" /> or a no-op-Disposable if
        /// <paramref name="disposable" /> was already disposed.
        /// </summary>
        internal static IDisposable GetValueOrDefault(ref IDisposable disposable)
        {
            var current = Volatile.Read(ref disposable);

            return BooleanDisposable.True == current
                ? EmptyDisposable.Instance
                : current;
        }

        /// <summary>
        /// Assigns <paramref name="value" /> to <paramref name="disposable" />.
        /// </summary>
        /// <returns>true if <paramref name="disposable" /> was assigned to <paramref name="value" /> and has not
        /// been assigned before.</returns>
        /// <returns>false if <paramref name="disposable" /> has been already disposed.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="disposable" /> has already been assigned a value.</exception>
        internal static bool SetSingle(ref IDisposable disposable, IDisposable value)
        {
            var result = TrySetSingle(ref disposable, value);

            if (TrySetSingleResult.AlreadyAssigned == result)
            {
                throw new InvalidOperationException();
            }

            return TrySetSingleResult.Success == result;
        }

        /// <summary>
        /// Tries to assign <paramref name="value" /> to <paramref name="disposable" />.
        /// </summary>
        /// <returns>A <see cref="TrySetSingleResult"/> value indicating the outcome of the operation.</returns>
        internal static TrySetSingleResult TrySetSingle(ref IDisposable disposable, IDisposable value)
        {
            var old = Interlocked.CompareExchange(ref disposable, value, null);

            if (null == old)
            {
                return TrySetSingleResult.Success;
            }

            if (BooleanDisposable.True != old)
            {
                return TrySetSingleResult.AlreadyAssigned;
            }

            value?.Dispose();

            return TrySetSingleResult.Disposed;
        }

        /// <summary>
        /// Tries to assign <paramref name="value" /> to <paramref name="disposable" />. If <paramref name="disposable" />
        /// is not disposed and is assigned a different value, it will not be disposed.
        /// </summary>
        /// <returns>true if <paramref name="value" /> was successfully assigned to <paramref name="disposable" />.</returns>
        /// <returns>false <paramref name="disposable" /> has been disposed.</returns>
        internal static bool TrySetMultiple(ref IDisposable disposable, IDisposable value)
        {
            // Let's read the current value atomically (also prevents reordering).
            var current = Volatile.Read(ref disposable);

            for (; ; )
            {
                // If it is the disposed instance, dispose the value.
                if (BooleanDisposable.True == current)
                {
                    value?.Dispose();
                    return false;
                }

                // Atomically swap in the new value and get back the old.
                var b = Interlocked.CompareExchange(ref disposable, value, current);

                // If the old and new are the same, the swap was successful and we can quit
                if (current == b)
                {
                    return true;
                }

                // Otherwise, make the old reference the current and retry.
                current = b;
            }
        }

        /// <summary>
        /// Tries to assign <paramref name="value" /> to <paramref name="fieldRef" />. If <paramref name="fieldRef" />
        /// is not disposed and is assigned a different value, it will be disposed.
        /// </summary>
        /// <returns>true if <paramref name="value" /> was successfully assigned to <paramref name="fieldRef" />.</returns>
        /// <returns>false <paramref name="fieldRef" /> has been disposed.</returns>
        internal static bool TrySetSerial(ref IDisposable fieldRef, IDisposable value)
        {
            var copy = Volatile.Read(ref fieldRef);

            for (; ; )
            {
                if (BooleanDisposable.True == copy)
                {
                    value?.Dispose();
                    return false;
                }

                var current = Interlocked.CompareExchange(ref fieldRef, value, copy);

                if (current == copy)
                {
                    copy?.Dispose();
                    return true;
                }

                copy = current;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <paramref name="fieldRef" /> has been disposed.
        /// </summary>
        /// <returns>true if <paramref name="fieldRef" /> has been disposed.</returns>
        /// <returns>false if <paramref name="fieldRef" /> has not been disposed.</returns>
        internal static bool GetIsDisposed(ref IDisposable fieldRef)
        {
            // We use a sentinel value to indicate we've been disposed. This sentinel never leaks
            // to the outside world (see the Disposable property getter), so no-one can ever assign
            // this value to us manually.
            return Volatile.Read(ref fieldRef) == BooleanDisposable.True;
        }

        /// <summary>
        /// Tries to dispose <paramref name="fieldRef" />. 
        /// </summary>
        /// <returns>true if <paramref name="fieldRef" /> was not disposed previously and was successfully disposed.</returns>
        /// <returns>false if <paramref name="fieldRef" /> was disposed previously.</returns>
        internal static bool TryDispose(ref IDisposable fieldRef)
        {
            var value = Interlocked.Exchange(ref fieldRef, BooleanDisposable.True);

            if (BooleanDisposable.True == value)
            {
                return false;
            }

            value?.Dispose();

            return true;
        }

        internal static bool TryRelease<TState>(
            ref IDisposable fieldRef,
            TState state,
            Action<IDisposable, TState> disposer)
        {
            var value = Interlocked.Exchange(ref fieldRef, BooleanDisposable.True);

            if (BooleanDisposable.True == value)
            {
                return false;
            }

            disposer.Invoke(value, state);

            return true;
        }
    }
}