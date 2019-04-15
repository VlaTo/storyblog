using System;
using System.Diagnostics;
using StoryBlog.Web.Blazor.Reactive.Concurrency;

namespace StoryBlog.Web.Blazor.Reactive
{
    //
    // WARNING: This code is kept *identically* in two places. One copy is kept in System.Reactive.Core for non-PLIB platforms.
    //          Another copy is kept in System.Reactive.PlatformServices to enlighten the default lowest common denominator
    //          behavior of Rx for PLIB when used on a more capable platform.
    //
    internal class /*Default*/StopwatchImpl : IStopwatch
    {
        private readonly Stopwatch stopwatch;

        public TimeSpan Elapsed => stopwatch.Elapsed;

        public StopwatchImpl()
        {
            stopwatch = Stopwatch.StartNew();
        }
    }
}