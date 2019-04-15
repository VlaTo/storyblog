using System;
using System.Collections.Generic;
using System.Threading;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on a designated thread.
    /// </summary>
    public sealed class EventLoopScheduler : LocalScheduler, ISchedulerPeriodic, IDisposable
    {
        /// <summary>
        /// Counter for diagnostic purposes, to name the threads.
        /// </summary>
        private static int counter;

        /// <summary>
        /// Thread factory function.
        /// </summary>
        private readonly Func<ThreadStart, Thread> threadFactory;

        /// <summary>
        /// Stopwatch for timing free of absolute time dependencies.
        /// </summary>
        private readonly IStopwatch stopwatch;

        /// <summary>
        /// Thread used by the event loop to run work items on. No work should be run on any other thread.
        /// If ExitIfEmpty is set, the thread can quit and a new thread will be created when new work is scheduled.
        /// </summary>
        private Thread thread;

        /// <summary>
        /// Gate to protect data structures, including the work queue and the ready list.
        /// </summary>
        private readonly object gate;

        /// <summary>
        /// Semaphore to count requests to re-evaluate the queue, from either Schedule requests or when a timer
        /// expires and moves on to the next item in the queue.
        /// </summary>
        private readonly SemaphoreSlim _evt;

        /// <summary>
        /// Queue holding work items. Protected by the gate.
        /// </summary>
        private readonly SchedulerQueue<TimeSpan> queue;

        /// <summary>
        /// Queue holding items that are ready to be run as soon as possible. Protected by the gate.
        /// </summary>
        private readonly Queue<ScheduledItem<TimeSpan>> readyList;

        /// <summary>
        /// Work item that will be scheduled next. Used upon reevaluation of the queue to check whether the next
        /// item is still the same. If not, a new timer needs to be started (see below).
        /// </summary>
        private ScheduledItem<TimeSpan> nextItem;

        /// <summary>
        /// Disposable that always holds the timer to dispatch the first element in the queue.
        /// </summary>
        private IDisposable nextTimer;

        /// <summary>
        /// Flag indicating whether the event loop should quit. When set, the event should be signaled as well to
        /// wake up the event loop thread, which will subsequently abandon all work.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Indicates whether the event loop thread is allowed to quit when no work is left. If new work
        /// is scheduled afterwards, a new event loop thread is created. This property is used by the
        /// NewThreadScheduler which uses an event loop for its recursive invocations.
        /// </summary>
        internal bool ExitIfEmpty
        {
            get;
            set;
        }

        /// <summary>
        /// Creates an object that schedules units of work on a designated thread.
        /// </summary>
        public EventLoopScheduler()
            : this(a => new Thread(a) { Name = "Event Loop " + Interlocked.Increment(ref counter), IsBackground = true })
        {
        }

#if !NO_THREAD
        /// <summary>
        /// Creates an object that schedules units of work on a designated thread, using the specified factory to control thread creation options.
        /// </summary>
        /// <param name="threadFactory">Factory function for thread creation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="threadFactory"/> is <c>null</c>.</exception>
        public EventLoopScheduler(Func<ThreadStart, Thread> threadFactory)
        {
#else
        internal EventLoopScheduler(Func<ThreadStart, Thread> threadFactory)
        {
#endif
            this.threadFactory = threadFactory ?? throw new ArgumentNullException(nameof(threadFactory));
            stopwatch = ConcurrencyAbstraction.Current.StartStopwatch();

            gate = new object();

            _evt = new SemaphoreSlim(0);
            queue = new SchedulerQueue<TimeSpan>();
            readyList = new Queue<ScheduledItem<TimeSpan>>();

            ExitIfEmpty = false;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <param name="dueTime">Relative time after which to execute the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">The scheduler has been disposed and doesn't accept new work.</exception>
        public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var due = stopwatch.Elapsed + dueTime;
            var si = new ScheduledItem<TimeSpan, TState>(this, state, action, due);

            lock (gate)
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("");
                }

                if (dueTime <= TimeSpan.Zero)
                {
                    readyList.Enqueue(si);
                    _evt.Release();
                }
                else
                {
                    queue.Enqueue(si);
                    _evt.Release();
                }

                EnsureThread();
            }

            return si;
        }

        /// <summary>
        /// Schedules a periodic piece of work on the designated thread.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        /// <exception cref="ObjectDisposedException">The scheduler has been disposed and doesn't accept new work.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new PeriodicallyScheduledWorkItem<TState>(this, state, period, action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
        {
            private readonly TimeSpan _period;
            private readonly Func<TState, TState> _action;
            private readonly EventLoopScheduler _scheduler;
            private readonly AsyncLock _gate = new AsyncLock();

            private TState _state;
            private TimeSpan _next;
            private IDisposable _task;

            public PeriodicallyScheduledWorkItem(EventLoopScheduler scheduler, TState state, TimeSpan period, Func<TState, TState> action)
            {
                _state = state;
                _period = period;
                _action = action;
                _scheduler = scheduler;
                _next = scheduler.stopwatch.Elapsed + period;

                Disposable.TrySetSingle(ref _task, scheduler.Schedule(this, _next - scheduler.stopwatch.Elapsed, (_, s) => s.Tick(_)));
            }

            private IDisposable Tick(IScheduler self)
            {
                _next += _period;

                Disposable.TrySetMultiple(ref _task, self.Schedule(this, _next - _scheduler.stopwatch.Elapsed, (_, s) => s.Tick(_)));

                _gate.Wait(
                    this,
                    closureWorkItem => closureWorkItem._state = closureWorkItem._action(closureWorkItem._state));

                return Disposable.Empty;
            }

            public void Dispose()
            {
                Disposable.TryDispose(ref _task);
                _gate.Dispose();
            }
        }

        /// <summary>
        /// Starts a new stopwatch object.
        /// </summary>
        /// <returns>New stopwatch object; started at the time of the request.</returns>
        public override IStopwatch StartStopwatch()
        {
            //
            // Strictly speaking, this explicit override is not necessary because the base implementation calls into
            // the enlightenment module to obtain the CAL, which would circle back to System.Reactive.PlatformServices
            // where we're currently running. This is merely a short-circuit to avoid the additional roundtrip.
            //
            return new StopwatchImpl();
        }

        /// <summary>
        /// Ends the thread associated with this scheduler. All remaining work in the scheduler queue is abandoned.
        /// </summary>
        public void Dispose()
        {
            lock (gate)
            {
                if (!disposed)
                {
                    disposed = true;
                    Disposable.TryDispose(ref nextTimer);
                    _evt.Release();
                }
            }
        }

        /// <summary>
        /// Ensures there is an event loop thread running. Should be called under the gate.
        /// </summary>
        private void EnsureThread()
        {
            if (thread == null)
            {
                thread = threadFactory(Run);
                thread.Start();
            }
        }

        /// <summary>
        /// Event loop scheduled on the designated event loop thread. The loop is suspended/resumed using the event
        /// which gets set by calls to Schedule, the next item timer, or calls to Dispose.
        /// </summary>
        private void Run()
        {
            while (true)
            {
                _evt.Wait();

                var ready = default(ScheduledItem<TimeSpan>[]);

                lock (gate)
                {
                    //
                    // Bug fix that ensures the number of calls to Release never greatly exceeds the number of calls to Wait.
                    // See work item #37: https://rx.codeplex.com/workitem/37
                    //
                    while (_evt.CurrentCount > 0)
                    {
                        _evt.Wait();
                    }

                    //
                    // The event could have been set by a call to Dispose. This takes priority over anything else. We quit the
                    // loop immediately. Subsequent calls to Schedule won't ever create a new thread.
                    //
                    if (disposed)
                    {
                        _evt.Dispose();
                        return;
                    }

                    while (queue.Count > 0 && queue.Peek().DueTime <= stopwatch.Elapsed)
                    {
                        var item = queue.Dequeue();
                        readyList.Enqueue(item);
                    }

                    if (queue.Count > 0)
                    {
                        var next = queue.Peek();
                        if (next != nextItem)
                        {
                            nextItem = next;

                            var due = next.DueTime - stopwatch.Elapsed;
                            Disposable.TrySetSerial(ref nextTimer, ConcurrencyAbstraction.Current.StartTimer(Tick, next, due));
                        }
                    }

                    if (readyList.Count > 0)
                    {
                        ready = readyList.ToArray();
                        readyList.Clear();
                    }
                }

                if (null != ready)
                {
                    foreach (var item in ready)
                    {
                        if (!item.IsCanceled)
                        {
                            item.Invoke();
                        }
                    }
                }

                if (ExitIfEmpty)
                {
                    lock (gate)
                    {
                        if (readyList.Count == 0 && queue.Count == 0)
                        {
                            thread = null;
                            return;
                        }
                    }
                }
            }
        }

        private void Tick(object state)
        {
            lock (gate)
            {
                if (!disposed)
                {
                    var item = (ScheduledItem<TimeSpan>)state;
                    if (item == nextItem)
                    {
                        nextItem = null;
                    }
                    if (queue.Remove(item))
                    {
                        readyList.Enqueue(item);
                    }

                    _evt.Release();
                }
            }
        }
    }
}