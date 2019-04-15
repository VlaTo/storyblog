using System;
using System.ComponentModel;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the current thread.
    /// </summary>
    /// <seealso cref="Scheduler.CurrentThread">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class CurrentThreadScheduler : LocalScheduler
    {
        private static readonly Lazy<CurrentThreadScheduler> StaticInstance;

        [ThreadStatic]
        private static SchedulerQueue<TimeSpan> threadLocalQueue;

        [ThreadStatic]
        private static IStopwatch clock;

        [ThreadStatic]
        private static bool running;

        /// <summary>
        /// Gets a value that indicates whether the caller must call a Schedule method.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public static bool IsScheduleRequired => false == running;

        /// <summary>
        /// Gets the singleton instance of the current thread scheduler.
        /// </summary>
        public static CurrentThreadScheduler Instance => StaticInstance.Value;

        private static TimeSpan Time
        {
            get
            {
                if (null == clock)
                {
                    clock = ConcurrencyAbstraction.Current.StartStopwatch();
                }

                return clock.Elapsed;
            }
        }

        private CurrentThreadScheduler()
        {
        }

        static CurrentThreadScheduler()
        {
            StaticInstance = new Lazy<CurrentThreadScheduler>(() => new CurrentThreadScheduler());
        }

        private static SchedulerQueue<TimeSpan> GetQueue() => threadLocalQueue;

        private static void SetQueue(SchedulerQueue<TimeSpan> newQueue) => threadLocalQueue = newQueue;

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            SchedulerQueue<TimeSpan> queue;

            // There is no timed task and no task is currently running
            if (false == running)
            {
                running = true;

                if (TimeSpan.Zero < dueTime)
                {
                    ConcurrencyAbstraction.Current.Sleep(dueTime);
                }

                // execute directly without queueing
                IDisposable disposable;

                try
                {
                    disposable = action(this, state);
                }
                catch
                {
                    SetQueue(null);
                    running = false;
                    throw;
                }

                // did recursive tasks arrive?
                queue = GetQueue();

                // yes, run those in the queue as well
                if (null != queue)
                {
                    try
                    {
                        Trampoline.Run(queue);
                    }
                    finally
                    {
                        SetQueue(null);
                        running = false;
                    }
                }
                else
                {
                    running = false;
                }

                return disposable;
            }

            queue = GetQueue();

            // if there is a task running or there is a queue
            if (null == queue)
            {
                queue = new SchedulerQueue<TimeSpan>(4);
                SetQueue(queue);
            }

            var dt = Time + Scheduler.Normalize(dueTime);

            // queue up more work
            var si = new ScheduledItem<TimeSpan, TState>(this, state, action, dt);

            queue.Enqueue(si);

            return si;
        }

        /// <summary>
        /// 
        /// </summary>
        private static class Trampoline
        {
            public static void Run(SchedulerQueue<TimeSpan> queue)
            {
                while (0 < queue.Count)
                {
                    var item = queue.Dequeue();

                    if (item.IsCanceled)
                    {
                        continue;
                    }

                    var wait = item.DueTime - Time;

                    if (wait.Ticks > 0)
                    {
                        ConcurrencyAbstraction.Current.Sleep(wait);
                    }

                    if (false == item.IsCanceled)
                    {
                        item.Invoke();
                    }
                }
            }
        }
    }
}