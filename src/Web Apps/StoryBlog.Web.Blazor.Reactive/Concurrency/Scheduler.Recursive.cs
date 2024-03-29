﻿using System;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    public static partial class Scheduler
    {
        /// <summary>
        /// Schedules an action to be executed recursively.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="action">Action to execute recursively. The parameter passed to the action is used to trigger recursive scheduling of the action.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
        {
            if (null == scheduler)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.Schedule(action, (_action, self) => _action(() => self(_action)));
        }

        /// <summary>
        /// Schedules an action to be executed recursively.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to execute recursively. The last parameter passed to the action is used to trigger recursive scheduling of the action, passing in recursive invocation state.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule<TState>(this IScheduler scheduler, TState state, Action<TState, Action<TState>> action)
        {
            if (null == scheduler)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.Schedule((state, action), (s, p) => InvokeRec1(s, p));
        }

        private static IDisposable InvokeRec1<TState>(IScheduler scheduler, (TState state, Action<TState, Action<TState>> action) tuple)
        {
            var recursiveInvoker = new InvokeRec1State<TState>(scheduler, tuple.action);
            recursiveInvoker.InvokeFirst(tuple.state);
            return recursiveInvoker;
        }

        /// <summary>
        /// Schedules an action to be executed recursively after a specified relative due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="action">Action to execute recursively. The parameter passed to the action is used to trigger recursive scheduling of the action at the specified relative time.</param>
        /// <param name="dueTime">Relative time after which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
        {
            if (null == scheduler)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.Schedule(action, dueTime, (_action, self) => _action(dt => self(_action, dt)));
        }

        /// <summary>
        /// Schedules an action to be executed recursively after a specified relative due time.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to execute recursively. The last parameter passed to the action is used to trigger recursive scheduling of the action, passing in the recursive due time and invocation state.</param>
        /// <param name="dueTime">Relative time after which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule<TState>(this IScheduler scheduler, TState state, TimeSpan dueTime, Action<TState, Action<TState, TimeSpan>> action)
        {
            if (null == scheduler)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.Schedule((state, action), dueTime, (s, p) => InvokeRec2(s, p));
        }

        private static IDisposable InvokeRec2<TState>(IScheduler scheduler, (TState state, Action<TState, Action<TState, TimeSpan>> action) tuple)
        {
            var recursiveInvoker = new InvokeRec2State<TState>(scheduler, tuple.action);
            recursiveInvoker.InvokeFirst(tuple.state);
            return recursiveInvoker;
        }

        /// <summary>
        /// Schedules an action to be executed recursively at a specified absolute due time.
        /// </summary>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="action">Action to execute recursively. The parameter passed to the action is used to trigger recursive scheduling of the action at the specified absolute time.</param>
        /// <param name="dueTime">Absolute time at which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
        {
            if (null == scheduler)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.Schedule(action, dueTime, (_action, self) => _action(dt => self(_action, dt)));
        }

        /// <summary>
        /// Schedules an action to be executed recursively at a specified absolute due time.
        /// </summary>
        /// <typeparam name="TState">The type of the state passed to the scheduled action.</typeparam>
        /// <param name="scheduler">Scheduler to execute the recursive action on.</param>
        /// <param name="state">State passed to the action to be executed.</param>
        /// <param name="action">Action to execute recursively. The last parameter passed to the action is used to trigger recursive scheduling of the action, passing in the recursive due time and invocation state.</param>
        /// <param name="dueTime">Absolute time at which to execute the action for the first time.</param>
        /// <returns>The disposable object used to cancel the scheduled action (best effort).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scheduler"/> or <paramref name="action"/> is <c>null</c>.</exception>
        public static IDisposable Schedule<TState>(this IScheduler scheduler, TState state, DateTimeOffset dueTime, Action<TState, Action<TState, DateTimeOffset>> action)
        {
            if (null == scheduler)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return scheduler.Schedule((state, action), dueTime, (s, p) => InvokeRec3(s, p));
        }

        private static IDisposable InvokeRec3<TState>(IScheduler scheduler, (TState state, Action<TState, Action<TState, DateTimeOffset>> action) tuple)
        {
            var recursiveInvoker = new InvokeRec3State<TState>(scheduler, tuple.action);
            recursiveInvoker.InvokeFirst(tuple.state);
            return recursiveInvoker;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        private abstract class InvokeRecBaseState<TState> : IDisposable
        {
            protected readonly IScheduler scheduler;

            protected readonly CompositeDisposable group;

            public InvokeRecBaseState(IScheduler scheduler)
            {
                this.scheduler = scheduler;
                group = new CompositeDisposable();
            }

            public void Dispose()
            {
                group.Dispose();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        private sealed class InvokeRec1State<TState> : InvokeRecBaseState<TState>
        {
            private readonly Action<TState, Action<TState>> action;
            private readonly Action<TState> recurseCallback;

            public InvokeRec1State(IScheduler scheduler, Action<TState, Action<TState>> action) : base(scheduler)
            {
                this.action = action;
                recurseCallback = state => InvokeNext(state);
            }

            internal void InvokeNext(TState state)
            {
                var sad = new SingleAssignmentDisposable();

                group.Add(sad);

                sad.Disposable = scheduler.Schedule((state, sad, @this: this), (_, nextState) =>
                {
                    nextState.@this.group.Remove(nextState.sad);
                    nextState.@this.InvokeFirst(nextState.state);
                    return Disposable.Empty;
                });
            }

            internal void InvokeFirst(TState state)
            {
                action(state, recurseCallback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        private sealed class InvokeRec2State<TState> : InvokeRecBaseState<TState>
        {
            private readonly Action<TState, Action<TState, TimeSpan>> action;
            private readonly Action<TState, TimeSpan> recurseCallback;

            public InvokeRec2State(IScheduler scheduler, Action<TState, Action<TState, TimeSpan>> action) : base(scheduler)
            {
                this.action = action;
                recurseCallback = (state, time) => InvokeNext(state, time);
            }

            internal void InvokeNext(TState state, TimeSpan time)
            {
                var sad = new SingleAssignmentDisposable();

                group.Add(sad);

                sad.Disposable = scheduler.Schedule((state, sad, @this: this), time, (_, nextState) =>
                {
                    nextState.@this.group.Remove(nextState.sad);
                    nextState.@this.InvokeFirst(nextState.state);
                    return Disposable.Empty;
                });
            }

            internal void InvokeFirst(TState state)
            {
                action(state, recurseCallback);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        private sealed class InvokeRec3State<TState> : InvokeRecBaseState<TState>
        {
            private readonly Action<TState, Action<TState, DateTimeOffset>> action;
            private readonly Action<TState, DateTimeOffset> recurseCallback;

            public InvokeRec3State(IScheduler scheduler, Action<TState, Action<TState, DateTimeOffset>> action) : base(scheduler)
            {
                this.action = action;
                recurseCallback = (state, dtOffset) => InvokeNext(state, dtOffset);
            }

            internal void InvokeNext(TState state, DateTimeOffset dtOffset)
            {
                var sad = new SingleAssignmentDisposable();

                group.Add(sad);

                sad.Disposable = scheduler.Schedule((state, sad, @this: this), dtOffset, (_, nextState) =>
                {
                    nextState.@this.group.Remove(nextState.sad);
                    nextState.@this.InvokeFirst(nextState.state);
                    return Disposable.Empty;
                });
            }

            internal void InvokeFirst(TState state)
            {
                action(state, recurseCallback);
            }
        }
    }
}