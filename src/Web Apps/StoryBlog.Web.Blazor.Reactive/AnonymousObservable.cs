using System;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// Class to create an <see cref="IObservable{T}"/> instance from a delegate-based implementation of the <see cref="IObservable{T}.Subscribe(IObserver{T})"/> method.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    public sealed class AnonymousObservable<T> : ObservableBase<T>
    {
        private readonly Func<IObserver<T>, IDisposable> subscribers;

        /// <summary>
        /// Creates an observable sequence object from the specified subscription function.
        /// </summary>
        /// <param name="subscribe"><see cref="IObservable{T}.Subscribe(IObserver{T})"/> method implementation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="subscribe"/> is <c>null</c>.</exception>
        public AnonymousObservable(Func<IObserver<T>, IDisposable> subscribe)
        {
            subscribers = subscribe ?? throw new ArgumentNullException(nameof(subscribe));
        }

        /// <summary>
        /// Calls the subscription function that was supplied to the constructor.
        /// </summary>
        /// <param name="observer">Observer to send notifications to.</param>
        /// <returns>Disposable object representing an observer's subscription to the observable sequence.</returns>
        protected override IDisposable SubscribeCore(IObserver<T> observer)
        {
            return subscribers(observer) ?? Disposable.Empty;
        }
    }
}