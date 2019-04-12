using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work to run immediately on the current thread.
    /// </summary>
    /// <seealso cref="Scheduler.Immediate">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class ImmediateScheduler : LocalScheduler
    {
        private static readonly Lazy<ImmediateScheduler> instance;

        /// <summary>
        /// Gets the singleton instance of the immediate scheduler.
        /// </summary>
        public static ImmediateScheduler Instance => instance.Value;

        private ImmediateScheduler()
        {
        }

        static ImmediateScheduler()
        {
            instance = new Lazy<ImmediateScheduler>(() => new ImmediateScheduler());
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return action(new AsyncLockScheduler(), state);
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is null.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var dt = Scheduler.Normalize(dueTime);

            if (dt.Ticks > 0)
            {
                ConcurrencyAbstraction.Current.Sleep(dt);
            }

            return action(new AsyncLockScheduler(), state);
        }

        private sealed class AsyncLockScheduler : LocalScheduler
        {
            private AsyncLock guard;

            public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
            {
                if (null == action)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                var m = new SingleAssignmentDisposable();

                if (null == guard)
                {
                    guard = new AsyncLock();
                }

                guard.Wait(
                    (@this: this, m, action, state),
                    tuple =>
                    {
                        if (!tuple.m.IsDisposed)
                        {
                            tuple.m.Disposable = tuple.action(tuple.@this, tuple.state);
                        }
                    });

                return m;
            }

            public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                if (null == action)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                if (0 >= dueTime.Ticks)
                {
                    return Schedule(state, action);
                }

                return ScheduleSlow(state, dueTime, action);
            }

            private IDisposable ScheduleSlow<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
            {
                var timer = ConcurrencyAbstraction.Current.StartStopwatch();

                var m = new SingleAssignmentDisposable();

                if (null == guard)
                {
                    guard = new AsyncLock();
                }

                guard.Wait(
                    (@this: this, m, state, action, timer, dueTime),
                    tuple =>
                    {
                        if (false == tuple.m.IsDisposed)
                        {
                            var sleep = tuple.dueTime - tuple.timer.Elapsed;

                            if (sleep.Ticks > 0)
                            {
                                ConcurrencyAbstraction.Current.Sleep(sleep);
                            }

                            if (false == tuple.m.IsDisposed)
                            {
                                tuple.m.Disposable = tuple.action(tuple.@this, tuple.state);
                            }
                        }
                    });

                return m;
            }
        }
    }
}