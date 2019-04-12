using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// Represents an awaitable scheduler operation. Awaiting the object causes the continuation to be posted back to the originating scheduler's work queue.
    /// </summary>
    public sealed class SchedulerOperation
    {
        private readonly Func<Action, IDisposable> schedule;
        private readonly CancellationToken cancellationToken;
        private readonly bool postBackToOriginalContext;

        internal SchedulerOperation(Func<Action, IDisposable> schedule, CancellationToken cancellationToken)
            : this(schedule, cancellationToken, false)
        {
        }

        internal SchedulerOperation(Func<Action, IDisposable> schedule, CancellationToken cancellationToken, bool postBackToOriginalContext)
        {
            this.schedule = schedule;
            this.cancellationToken = cancellationToken;
            this.postBackToOriginalContext = postBackToOriginalContext;
        }

        /// <summary>
        /// Controls whether the continuation is run on the originating synchronization context (false by default).
        /// </summary>
        /// <param name="continueOnCapturedContext">true to run the continuation on the captured synchronization context; false otherwise (default).</param>
        /// <returns>Scheduler operation object with configured await behavior.</returns>
        public SchedulerOperation ConfigureAwait(bool continueOnCapturedContext)
        {
            return new SchedulerOperation(schedule, cancellationToken, continueOnCapturedContext);
        }

        /// <summary>
        /// Gets an awaiter for the scheduler operation, used to post back the continuation.
        /// </summary>
        /// <returns>Awaiter for the scheduler operation.</returns>
        public SchedulerOperationAwaiter GetAwaiter()
        {
            return new SchedulerOperationAwaiter(schedule, cancellationToken, postBackToOriginalContext);
        }
    }

    /// <summary>
    /// (Infrastructure) Scheduler operation awaiter type used by the code generated for C# await and Visual Basic Await expressions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class SchedulerOperationAwaiter : INotifyCompletion
    {
        private readonly Func<Action, IDisposable> schedule;
        private readonly CancellationToken cancellationToken;
        private readonly bool postBackToOriginalContext;
        private readonly CancellationTokenRegistration ctr;
        private volatile Action continuation;
        private volatile IDisposable disposable;

        /// <summary>
        /// Indicates whether the scheduler operation has completed. Returns false unless cancellation was already requested.
        /// </summary>
        public bool IsCompleted => cancellationToken.IsCancellationRequested;

        /// <summary>
        /// Completes the scheduler operation, throwing an OperationCanceledException in case cancellation was requested.
        /// </summary>
        public void GetResult() => cancellationToken.ThrowIfCancellationRequested();

        internal SchedulerOperationAwaiter(Func<Action, IDisposable> schedule, CancellationToken cancellationToken, bool postBackToOriginalContext)
        {
            this.schedule = schedule;
            this.cancellationToken = cancellationToken;
            this.postBackToOriginalContext = postBackToOriginalContext;

            if (cancellationToken.CanBeCanceled)
            {
                ctr = this.cancellationToken.Register(@this => ((SchedulerOperationAwaiter)@this).Cancel(), this);
            }
        }

        /// <summary>
        /// Registers the continuation with the scheduler operation.
        /// </summary>
        /// <param name="complete">Continuation to be run on the originating scheduler.</param>
        public void OnCompleted(Action complete)
        {
            if (null == complete)
            {
                throw new ArgumentNullException(nameof(complete));
            }

            if (null == continuation)
            {
                throw new InvalidOperationException();
            }

            if (postBackToOriginalContext)
            {
                var ctx = SynchronizationContext.Current;

                if (null != ctx)
                {
                    var original = complete;

                    complete = () =>
                    {
                        //
                        // No need for OperationStarted and OperationCompleted calls here;
                        // this code is invoked through await support and will have a way
                        // to observe its start/complete behavior, either through returned
                        // Task objects or the async method builder's interaction with the
                        // SynchronizationContext object.
                        //
                        // In general though, Rx doesn't play nicely with synchronization
                        // contexts objects at the scheduler level. It's possible to start
                        // async operations by calling Schedule, without a way to observe
                        // their completion. Not interacting with SynchronizationContext
                        // is a conscious design decision as the performance impact was non
                        // negligible and our schedulers abstract over more constructs.
                        //
                        ctx.Post(a => ((Action)a)(), original);
                    };
                }
            }

            var ran = 0;

            continuation = () =>
            {
                if (Interlocked.Exchange(ref ran, 1) == 0)
                {
                    ctr.Dispose(); // no null-check needed (struct)
                    complete.Invoke();
                }
            };

            disposable = schedule.Invoke(continuation);
        }

        private void Cancel()
        {
            disposable?.Dispose();
            continuation?.Invoke();
        }
    }
}