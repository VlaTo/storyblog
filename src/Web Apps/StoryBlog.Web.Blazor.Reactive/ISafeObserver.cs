﻿using System;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// Base interface for observers that can dispose of a resource on a terminal notification
    /// or when disposed itself.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface ISafeObserver<in T> : IObserver<T>, IDisposable
    {
        void SetResource(IDisposable resource);
    }
}