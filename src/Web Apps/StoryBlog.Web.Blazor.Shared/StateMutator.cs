using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public static class StateMutator
    {
        private static readonly ConcurrentDictionary<Type, IStateMutator> mutators;

        static StateMutator()
        {
            mutators = new ConcurrentDictionary<Type, IStateMutator>();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IStateMutator For<TState>()
            where TState : IMutableState
        {
            return For(typeof(TState));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        public static IStateMutator For(Type stateType) =>
            mutators.GetOrAdd(stateType, notused => CreateMutator(stateType));

        private static IStateMutator CreateMutator(Type stateType) =>
            New.CreateGenericInstance(typeof(StateMutator<>), stateType) as IStateMutator;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class StateMutator<TState> : IStateMutator
        where TState : IMutableState, new()
    {
        private readonly Dictionary<Type, Action<TState, object>> mutators;

        /// <summary>
        /// 
        /// </summary>
        public StateMutator()
        {
            mutators = new Dictionary<Type, Action<TState, object>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mutableState"></param>
        /// <param name="mutator"></param>
        public void Mutate(IMutableState mutableState, IMutator mutator)
        {
            if (false == mutators.TryGetValue(mutator.GetType(), out var action))
            {
                throw new NotImplementedException();
            }

            action.Invoke((TState) mutableState, mutator);
        }
    }
}