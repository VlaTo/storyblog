﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    internal class ConcurrencyAbstractionLayerImpl : IConcurrencyAbstraction
    {
        private sealed class WorkItem
        {
            public Action<object> Action
            {
                get;
            }

            public object State
            {
                get;
            }

            public WorkItem(Action<object> action, object state)
            {
                Action = action;
                State = state;
            }
        }

        public IDisposable StartTimer(Action<object> action, object state, TimeSpan dueTime) => new Timer(action, state, Normalize(dueTime));

        public IDisposable StartPeriodicTimer(Action action, TimeSpan period)
        {
            if (period < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            //
            // The contract for periodic scheduling in Rx is that specifying TimeSpan.Zero as the period causes the scheduler to 
            // call back periodically as fast as possible, sequentially.
            //
            if (period == TimeSpan.Zero)
            {
                return new FastPeriodicTimer(action);
            }

            return new PeriodicTimer(action, period);
        }

        public IDisposable QueueUserWorkItem(Action<object> action, object state)
        {
            ThreadPool.QueueUserWorkItem(itemObject =>
            {
                var item = (WorkItem)itemObject;

                item.Action(item.State);
            }, new WorkItem(action, state));

            return Disposable.Empty;
        }

        public void Sleep(TimeSpan timeout) => Thread.Sleep(Normalize(timeout));

        public IStopwatch StartStopwatch() => new StopwatchImpl();

        public bool SupportsLongRunning => true;

        public void StartThread(Action<object> action, object state)
        {
            new Thread(itemObject =>
            {
                var item = (WorkItem)itemObject;

                item.Action(item.State);
            })
            { IsBackground = true }.Start(new WorkItem(action, state));
        }

        private static TimeSpan Normalize(TimeSpan dueTime) => dueTime < TimeSpan.Zero ? TimeSpan.Zero : dueTime;

        //
        // Some historical context. In the early days of Rx, we discovered an issue with
        // the rooting of timers, causing them to get GC'ed even when the IDisposable of
        // a scheduled activity was kept alive. The original code simply created a timer
        // as follows:
        //
        //   var t = default(Timer);
        //   t = new Timer(_ =>
        //   {
        //       t = null;
        //       Debug.WriteLine("Hello!");
        //   }, null, 5000, Timeout.Infinite);
        //
        // IIRC the reference to "t" captured by the closure wasn't sufficient on .NET CF
        // to keep the timer rooted, causing problems on Windows Phone 7. As a result, we
        // added rooting code using a dictionary (SD 7280), which we carried forward all
        // the way to Rx v2.0 RTM.
        //
        // However, the desktop CLR's implementation of System.Threading.Timer exhibits
        // other characteristics where a timer can root itself when the timer is still
        // reachable through the state or callback parameters. To illustrate this, run
        // the following piece of code:
        //
        //   static void Main()
        //   {
        //       Bar();
        //   
        //       while (true)
        //       {
        //           GC.Collect();
        //           GC.WaitForPendingFinalizers();
        //           Thread.Sleep(100);
        //       }
        //   }
        //   
        //   static void Bar()
        //   {
        //       var t = default(Timer);
        //       t = new Timer(_ =>
        //       {
        //           t = null; // Comment out this line to see the timer stop
        //           Console.WriteLine("Hello!");
        //       }, null, 5000, Timeout.Infinite);
        //   }
        //
        // When the closure over "t" is removed, the timer will stop automatically upon
        // garbage collection. However, when retaining the reference, this problem does
        // not exist. The code below exploits this behavior, avoiding unnecessary costs
        // to root timers in a thread-safe manner.
        //
        // Below is a fragment of SOS output, proving the proper rooting:
        //
        //   !gcroot 02492440
        //    HandleTable:
        //        005a13fc (pinned handle)
        //        -> 03491010 System.Object[]
        //        -> 024924dc System.Threading.TimerQueue
        //        -> 02492450 System.Threading.TimerQueueTimer
        //        -> 02492420 System.Threading.TimerCallback
        //        -> 02492414 TimerRootingExperiment.Program+<>c__DisplayClass1
        //        -> 02492440 System.Threading.Timer
        //
        // With the USE_TIMER_SELF_ROOT symbol, we shake off this additional rooting code
        // for newer platforms where this no longer needed. We checked this on .NET Core
        // as well as .NET 4.0, and only #define this symbol for those platforms.
        //
        // NB: 4/13/2017 - All target platforms for the 4.x release have the self-rooting
        //                 behavior described here, so we removed the USE_TIMER_SELF_ROOT
        //                 symbol.
        //

        private sealed class Timer : IDisposable
        {
            private volatile object _state;
            private Action<object> _action;
            private IDisposable _timer;

            private static readonly object DisposedState = new object();

            public Timer(Action<object> action, object state, TimeSpan dueTime)
            {
                _state = state;
                _action = action;

                Disposable.SetSingle(ref _timer, new System.Threading.Timer(_ => Tick(_), this, dueTime, TimeSpan.FromMilliseconds(Timeout.Infinite)));
            }

            private static void Tick(object state)
            {
                var timer = (Timer)state;

                try
                {
                    var timerState = timer._state;
                    if (timerState != DisposedState)
                    {
                        timer._action(timerState);
                    }
                }
                finally
                {
                    Disposable.TryDispose(ref timer._timer);
                }
            }

            public void Dispose()
            {
                if (Disposable.TryDispose(ref _timer))
                {
                    _action = Stubs<object>.Ignore;
                    _state = DisposedState;
                }
            }
        }

        private sealed class PeriodicTimer : IDisposable
        {
            private Action _action;
            private volatile System.Threading.Timer _timer;

            public PeriodicTimer(Action action, TimeSpan period)
            {
                _action = action;

                //
                // Rooting of the timer happens through the timer's state
                // which is the current instance and has a field to store the Timer instance.
                //
                _timer = new System.Threading.Timer(_ => Tick(_), this, period, period);
            }

            private static void Tick(object state)
            {
                var timer = (PeriodicTimer)state;

                timer._action();
            }

            public void Dispose()
            {
                var timer = _timer;
                if (timer != null)
                {
                    _action = Stubs.Nop;
                    _timer = null;

                    timer.Dispose();
                }
            }
        }

        private sealed class FastPeriodicTimer : IDisposable
        {
            private readonly Action _action;
            private volatile bool _disposed;

            public FastPeriodicTimer(Action action)
            {
                _action = action;

                new Thread(_ => Loop(_))
                {
                    Name = "Rx-FastPeriodicTimer",
                    IsBackground = true
                }
                .Start(this);
            }

            private static void Loop(object threadParam)
            {
                var timer = (FastPeriodicTimer)threadParam;

                while (!timer._disposed)
                {
                    timer._action();
                }
            }

            public void Dispose()
            {
                _disposed = true;
            }
        }
    }
}