using System;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// Represents an object that schedules units of work on the platform's default scheduler.
    /// </summary>
    /// <seealso cref="Scheduler.Default">Singleton instance of this type exposed through this static property.</seealso>
    public sealed class DefaultScheduler : LocalScheduler, ISchedulerPeriodic
    {
        private static readonly Lazy<DefaultScheduler> instance;
        private static readonly IConcurrencyAbstraction concurrency;

        /// <summary>
        /// Gets the singleton instance of the default scheduler.
        /// </summary>
        public static DefaultScheduler Instance => instance.Value;

        private DefaultScheduler()
        {
        }

        static DefaultScheduler()
        {
            instance = new Lazy<DefaultScheduler>(() => new DefaultScheduler());
            concurrency = ConcurrencyAbstraction.Current;
        }

        /// <summary>
        /// Schedules an action to be executed.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to be executed.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public override IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var workItem = new UserWorkItem<TState>(this, state, action);

            workItem.CancelQueueDisposable = concurrency.QueueUserWorkItem(
                closureWorkItem => ((UserWorkItem<TState>) closureWorkItem).Run(),
                workItem
            );

            return workItem;
        }

        /// <summary>
        /// Schedules an action to be executed after dueTime, using a System.Threading.Timer object.
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

            var dt = Scheduler.Normalize(dueTime);

            if (0 == dt.Ticks)
            {
                return Schedule(state, action);
            }

            var workItem = new UserWorkItem<TState>(this, state, action);

            workItem.CancelQueueDisposable = concurrency.StartTimer(
                closureWorkItem => ((UserWorkItem<TState>) closureWorkItem).Run(),
                workItem,
                dt
            );

            return workItem;
        }

        /// <summary>
        /// Schedules a periodic piece of work, using a System.Threading.Timer object.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="state">Initial state passed to the action upon the first iteration.</param>
        /// <param name="period">Period for running the work periodically.</param>
        /// <param name="action">Action to be executed, potentially updating the state.</param>
        /// <returns>The disposable object used to cancel the scheduled recurring action (best effort).</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="period"/> is less than <see cref="TimeSpan.Zero"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public IDisposable SchedulePeriodic<TState>(TState state, TimeSpan period, Func<TState, TState> action)
        {
            if (TimeSpan.Zero > period)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return new PeriodicallyScheduledWorkItem<TState>(state, period, action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        private sealed class PeriodicallyScheduledWorkItem<TState> : IDisposable
        {
            private TState state;
            private Func<TState, TState> action;
            private readonly IDisposable cancel;
            private readonly AsyncLock gate = new AsyncLock();

            public PeriodicallyScheduledWorkItem(TState state, TimeSpan period, Func<TState, TState> action)
            {
                this.state = state;
                this.action = action;

                cancel = concurrency.StartPeriodicTimer(Tick, period);
            }

            private void Tick()
            {
                gate.Wait(
                    this,
                    closureWorkItem => closureWorkItem.state = closureWorkItem.action(closureWorkItem.state)
                );
            }

            public void Dispose()
            {
                cancel.Dispose();
                gate.Dispose();
                action = Stubs<TState>.I;
            }
        }
        
        /// <summary>
        /// Discovers scheduler services by interface type.
        /// </summary>
        /// <param name="serviceType">Scheduler service interface type to discover.</param>
        /// <returns>Object implementing the requested service, if available; null otherwise.</returns>
        protected override object GetService(Type serviceType)
        {
            if (serviceType == typeof(ISchedulerLongRunning))
            {
                if (concurrency.SupportsLongRunning)
                {
                    return LongRunning.Instance;
                }
            }

            return base.GetService(serviceType);
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class LongRunning : ISchedulerLongRunning
        {
            public static ISchedulerLongRunning Instance = new LongRunning();

            private sealed class LongScheduledWorkItem<TState> : ICancelable
            {
                private readonly TState state;
                private readonly Action<TState, ICancelable> action;
                private IDisposable cancel;

                public bool IsDisposed => Disposable.GetIsDisposed(ref cancel);

                public LongScheduledWorkItem(TState state, Action<TState, ICancelable> action)
                {
                    this.state = state;
                    this.action = action;

                    concurrency.StartThread(
                        @thisObject =>
                        {
                            var @this = (LongScheduledWorkItem<TState>)@thisObject;

                            //
                            // Notice we don't check d.IsDisposed. The contract for ISchedulerLongRunning
                            // requires us to ensure the scheduled work gets an opportunity to observe
                            // the cancellation request.
                            //
                            @this.action(@this.state, @this);
                        },
                        this
                    );
                }

                public void Dispose()
                {
                    Disposable.TryDispose(ref cancel);
                }
            }

            public IDisposable ScheduleLongRunning<TState>(TState state, Action<TState, ICancelable> action)
            {
                if (null == action)
                {
                    throw new ArgumentNullException(nameof(action));
                }

                return new LongScheduledWorkItem<TState>(state, action);
            }
        }
    }
}