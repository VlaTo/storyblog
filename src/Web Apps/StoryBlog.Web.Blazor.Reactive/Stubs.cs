﻿using System;
using System.Runtime.ExceptionServices;

namespace StoryBlog.Web.Blazor.Reactive
{
    internal static class Stubs<T>
    {
        public static readonly Action<T> Ignore = _ => { };
        public static readonly Func<T, T> I = _ => _;
    }

    internal static class Stubs
    {
        public static readonly Action Nop = () => { };
        //public static readonly Action<Exception> Throw = ex => { ex.Throw(); };
        public static readonly Action<Exception> Throw = ex => ExceptionDispatchInfo.Capture(ex).Throw();
    }

#if !NO_THREAD
    internal static class TimerStubs
    {
#if NETSTANDARD1_3
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { }, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
#else
        public static readonly System.Threading.Timer Never = new System.Threading.Timer(_ => { });
#endif
    }
#endif
}