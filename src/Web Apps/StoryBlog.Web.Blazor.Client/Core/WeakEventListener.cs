using System;

namespace StoryBlog.Web.Client.Core
{
    internal interface IWeakEventListener
    {
        bool IsAlive
        {
            get;
        }

        object Source
        {
            get;
        }

        Delegate Handler
        {
            get;
        }

        void StopListening();
    }

    internal abstract class WeakEventListenerBase<T, TArgs> : IWeakEventListener
        where T : class
    {
        private readonly WeakReference<T> source;
        private readonly WeakReference<Action<T, TArgs>> handler;

        public bool IsAlive => handler.TryGetTarget(out _) && source.TryGetTarget(out var __);

        public object Source => source.TryGetTarget(out var value) ? value : null;

        public Delegate Handler => handler.TryGetTarget(out var value) ? value : null;

        protected WeakEventListenerBase(T source, Action<T, TArgs> handler)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (null == handler)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            this.source = new WeakReference<T>(source);
            this.handler = new WeakReference<Action<T, TArgs>>(handler);
        }

        protected void HandleEvent(object sender, TArgs e)
        {
            if (handler.TryGetTarget(out var target))
            {
                target.Invoke(sender as T, e);
            }
            else
            {
                StopListening();
            }
        }

        public void StopListening()
        {
            if (source.TryGetTarget(out var value))
            {
                StopListening(value);
            }
        }

        protected abstract void StopListening(T source);
    }

    internal class TypedWeakEventListener<T, TArgs> : WeakEventListenerBase<T, TArgs>
        where T : class
    {
        private readonly Action<T, EventHandler<TArgs>> unregister;

        public TypedWeakEventListener(
            T source,
            Action<T, EventHandler<TArgs>> register,
            Action<T, EventHandler<TArgs>> unregister,
            Action<T, TArgs> handler)
            : base(source, handler)
        {
            if (null == register)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (null == unregister)
            {
                throw new ArgumentNullException(nameof(unregister));
            }

            this.unregister = unregister;

            register.Invoke(source, HandleEvent);
        }

        protected override void StopListening(T source) => unregister.Invoke(source, HandleEvent);
    }

    internal class TypedWeakEventListener<T, TArgs, THandler> : WeakEventListenerBase<T, TArgs>
        where T : class
        where TArgs : EventArgs
        where THandler : Delegate
    {
        private readonly Action<T, THandler> unregister;
        private readonly THandler target;

        public TypedWeakEventListener(T source, Action<T, THandler> register, Action<T, THandler> unregister,
            Action<T, TArgs> handler)
            : base(source, handler)
        {
            if (null == register)
            {
                throw new ArgumentNullException(nameof(register));
            }

            if (null == unregister)
            {
                throw new ArgumentNullException(nameof(unregister));
            }

            this.unregister = unregister;
            target = (THandler) Delegate.CreateDelegate(typeof(THandler), this, nameof(HandleEvent));

            register.Invoke(source, target);
        }

        protected override void StopListening(T source)
        {
            unregister.Invoke(source, target);
        }
    }
}