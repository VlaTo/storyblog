using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Reactive.Concurrency
{
    /// <summary>
    /// (Infrastructure) Monitors for system clock changes based on a periodic timer.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PeriodicTimerSystemClockMonitor : INotifySystemClockChanged
    {
        private const int SyncMaxRetries = 100;
        private const double SyncMaxDelta = 10;
        private const int MaxError = 100;

        /// <summary>
        /// Use the Unix milliseconds for the current time
        /// so it can be atomically read/written without locking.
        /// </summary>
        private long lastTimeMillis;
        private readonly TimeSpan period;
        private IDisposable timer;
        private EventHandler<SystemClockChangedEventArgs> systemClockChanged;

        /// <summary>
        /// Event that gets raised when a system clock change is detected.
        /// </summary>
        public event EventHandler<SystemClockChangedEventArgs> SystemClockChanged
        {
            add
            {
                NewTimer();

                systemClockChanged += value;
            }

            remove
            {
                systemClockChanged -= value;

                Disposable.TrySetSerial(ref timer, Disposable.Empty);
            }
        }

        /// <summary>
        /// Creates a new monitor for system clock changes with the specified polling frequency.
        /// </summary>
        /// <param name="period">Polling frequency for system clock changes.</param>
        public PeriodicTimerSystemClockMonitor(TimeSpan period)
        {
            this.period = period;
        }

        private void NewTimer()
        {
            Disposable.TrySetSerial(ref timer, Disposable.Empty);

            var n = 0L;

            for (; ; )
            {
                var now = SystemClock.UtcNow.ToUnixTimeMilliseconds();

                Interlocked.Exchange(ref lastTimeMillis, now);
                Disposable.TrySetSerial(ref timer, ConcurrencyAbstraction.Current.StartPeriodicTimer(TimeChanged, period));

                if (Math.Abs(SystemClock.UtcNow.ToUnixTimeMilliseconds() - now) <= SyncMaxDelta)
                {
                    break;
                }

                if (Volatile.Read(ref timer) == Disposable.Empty)
                {
                    break;
                }

                if (++n >= SyncMaxRetries)
                {
                    Task.Delay((int)SyncMaxDelta).Wait();
                }
            }
        }

        private void TimeChanged()
        {
            var newTime = SystemClock.UtcNow;
            var now = newTime.ToUnixTimeMilliseconds();
            var last = Volatile.Read(ref lastTimeMillis);

            var oldTime = (long)(last + period.TotalMilliseconds);
            var diff = now - oldTime;

            if (Math.Abs(diff) >= MaxError)
            {
                systemClockChanged?.Invoke(
                    this,
                    new SystemClockChangedEventArgs(DateTimeOffset.FromUnixTimeMilliseconds(oldTime), newTime)
                );

                NewTimer();
            }
            else
            {
                Interlocked.Exchange(ref lastTimeMillis, SystemClock.UtcNow.ToUnixTimeMilliseconds());
            }
        }
    }
}