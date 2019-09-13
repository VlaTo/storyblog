using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Client.Core
{
    public class WeakEventManager
    {
        private Dictionary<IWeakEventListener, Delegate> listeners = new Dictionary<IWeakEventListener, Delegate>();

        /// <summary>
        /// Registers the given delegate as a handler for the event specified by lamba expressions for registering and unregistering the event
        /// </summary>
        public void AddWeakEventListener<T, TArgs, THandler>(
            T source,
            Action<T, THandler> register,
            Action<T, THandler> unregister,
            Action<T, TArgs> handler)
            where T : class
            where TArgs : EventArgs
            where THandler : Delegate
        {
            listeners.Add(
                new TypedWeakEventListener<T, TArgs, THandler>(source, register, unregister, handler),
                handler
            );
        }

        /// <summary>
        /// Registers the given delegate as a handler for the event specified by lamba expressions for registering and unregistering the event
        /// </summary>
        public void AddWeakEventListener<T, TArgs>(
            T source,
            Action<T, EventHandler<TArgs>> register,
            Action<T, EventHandler<TArgs>> unregister,
            Action<T, TArgs> handler)
            where T : class
        {
            listeners.Add(
                new TypedWeakEventListener<T, TArgs>(source, register, unregister, handler),
                handler
            );
        }

        /// <summary>
        /// Unregisters any previously registered weak event handlers on the given source object
        /// </summary>
        public void RemoveWeakEventListener<T>(T source)
            where T : class
        {
            var toRemove = new List<IWeakEventListener>(listeners.Count);

            foreach (var listener in listeners.Keys)
            {
                if (!listener.IsAlive)
                {
                    toRemove.Add(listener);
                }
                else if (listener.Source == source)
                {
                    listener.StopListening();
                    toRemove.Add(listener);
                }
            }

            foreach (var item in toRemove)
            {
                listeners.Remove(item);
            }
        }

        /// <summary>
        /// Unregisters all weak event listeners that have been registered by this weak event manager instance
        /// </summary>
        public void ClearWeakEventListeners()
        {
            foreach (var listener in listeners.Keys)
            {
                if (listener.IsAlive)
                {
                    listener.StopListening();
                }
            }

            listeners.Clear();
        }
    }
}