using System;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    internal sealed class UserWorkItem<TState> : IDisposable
    {
        private IDisposable cancelRunDisposable;
        private IDisposable cancelQueueDisposable;

        private readonly TState state;
        private readonly IScheduler scheduler;
        private readonly Func<IScheduler, TState, IDisposable> action;

        public UserWorkItem(IScheduler scheduler, TState state, Func<IScheduler, TState, IDisposable> action)
        {
            this.state = state;
            this.action = action;
            this.scheduler = scheduler;
        }

        public void Run()
        {
            if (!Disposable.GetIsDisposed(ref cancelRunDisposable))
            {
                Disposable.SetSingle(ref cancelRunDisposable, action(scheduler, state));
            }
        }

        public IDisposable CancelQueueDisposable
        {
            get => Disposable.GetValue(ref cancelQueueDisposable);
            set => Disposable.SetSingle(ref cancelQueueDisposable, value);
        }

        public void Dispose()
        {
            Disposable.TryDispose(ref cancelQueueDisposable);
            Disposable.TryDispose(ref cancelRunDisposable);
        }
    }
}