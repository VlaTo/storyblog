﻿using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// Asynchronous lock.
    /// </summary>
    public sealed class AsyncLock : IDisposable
    {
        private bool acquired;
        private bool faulted;
        private readonly object guard = new object();
        private Queue<(Action<Delegate, object> action, Delegate @delegate, object state)> queue;

        /// <summary>
        /// Queues the action for execution. If the caller acquires the lock and becomes the owner,
        /// the queue is processed. If the lock is already owned, the action is queued and will get
        /// processed by the owner.
        /// </summary>
        /// <param name="action">Action to queue for execution.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        public void Wait(Action action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Wait(action, closureAction => closureAction.Invoke());
        }

        /// <summary>
        /// Queues the action for execution. If the caller acquires the lock and becomes the owner,
        /// the queue is processed. If the lock is already owned, the action is queued and will get
        /// processed by the owner.
        /// </summary>
        /// <param name="action">Action to queue for execution.</param>
        /// <param name="state">The state to pass to the action when it gets invoked under the lock.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
        /// <remarks>In case TState is a value type, this operation will involve boxing of <paramref name="state"/>.
        /// However, this is often an improvement over the allocation of a closure object and a delegate.</remarks>
        internal void Wait<TState>(TState state, Action<TState> action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            Wait(state, action, (actionObject, stateObject) => ((Action<TState>)actionObject)((TState)stateObject));
        }

        private void Wait(object state, Delegate @delegate, Action<Delegate, object> action)
        {
            // allow one thread to update the state
            lock (guard)
            {
                // if a previous action crashed, ignore any future actions
                if (faulted)
                {
                    return;
                }

                // if the "lock" is busy, queue up the extra work
                // otherwise there is no need to queue up "action"
                if (acquired)
                {
                    // create the queue if necessary
                    var q = queue;

                    if (null == q)
                    {
                        q = new Queue<(Action<Delegate, object> action, Delegate @delegate, object state)>();
                        queue = q;
                    }

                    // enqueue the work
                    q.Enqueue((action, @delegate, state));

                    return;
                }

                // indicate there is processing going on
                acquired = true;
            }

            // if we get here, execute the "action" first

            for (; ; )
            {
                try
                {
                    action(@delegate, state);
                }
                catch
                {
                    // the execution failed, terminate this AsyncLock
                    lock (guard)
                    {
                        // throw away the queue
                        queue = null;
                        // report fault
                        faulted = true;
                    }
                    throw;
                }

                // execution succeeded, let's see if more work has to be done
                lock (guard)
                {
                    var q = queue;
                    
                    // either there is no queue yet or we run out of work
                    if (null == q || 0 == q.Count)
                    {
                        // release the lock
                        acquired = false;
                        return;
                    }

                    // get the next work action
                    (action, @delegate, state) = q.Dequeue();
                }
                // loop back and execute the action
            }
        }

        /// <summary>
        /// Clears the work items in the queue and drops further work being queued.
        /// </summary>
        public void Dispose()
        {
            lock (guard)
            {
                queue = null;
                faulted = true;
            }
        }
    }
}