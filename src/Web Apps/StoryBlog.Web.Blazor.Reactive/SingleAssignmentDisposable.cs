using System;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// Represents a disposable resource which only allows a single assignment of its underlying disposable resource.
    /// If an underlying disposable resource has already been set, future attempts to set the underlying disposable resource will throw an <see cref="InvalidOperationException"/>.
    /// </summary>
    public sealed class SingleAssignmentDisposable : ICancelable
    {
        private IDisposable current;

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => Reactive.Disposable.GetIsDisposed(ref current);

        /// <summary>
        /// Gets or sets the underlying disposable. After disposal, the result of getting this property is undefined.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="SingleAssignmentDisposable"/> has already been assigned to.</exception>
        public IDisposable Disposable
        {
            get => Reactive.Disposable.GetValueOrDefault(ref current);
            set => Reactive.Disposable.SetSingle(ref current, value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleAssignmentDisposable"/> class.
        /// </summary>
        public SingleAssignmentDisposable()
        {
        }

        /// <summary>
        /// Disposes the underlying disposable.
        /// </summary>
        public void Dispose()
        {
            Reactive.Disposable.TryDispose(ref current);
        }
    }
}