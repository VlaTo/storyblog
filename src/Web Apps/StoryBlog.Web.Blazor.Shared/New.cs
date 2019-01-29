using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace StoryBlog.Web.Blazor.Shared
{
    public static class New<TClass>
        where TClass : class
    {
        public static readonly Func<TClass> Instance =
            Expression.Lambda<Func<TClass>>(Expression.New(typeof(TClass))).Compile();

        public static Func<TClass> GetInstance(TClass type) => Instance;

        private class WithCtorParam<TParam>
        {
            private static readonly Dictionary<Type, Func<TParam, TClass>> cache =
                new Dictionary<Type, Func<TParam, TClass>>();

            public static TClass Instance(TParam argument)
            {
                if (cache.TryGetValue(typeof(TClass), out var func))
                {
                    return func.Invoke(argument);
                }

                var ctor = typeof(TClass).GetConstructor(new[] {typeof(TParam)});
                var @parameter = Expression.Parameter(typeof(TParam));
                var @function = Expression
                    .Lambda<Func<TParam, TClass>>(Expression.New(ctor, @parameter), @parameter)
                    .Compile();

                cache.Add(typeof(TClass), @function);

                return @function.Invoke(argument);
            }
        }
    }

    public static class New
    {
        private static readonly Dictionary<Tuple<Type, Type>, Func<object>> cache =
            new Dictionary<Tuple<Type, Type>, Func<object>>();

        public static object CreateGenericInstance(Type genericType, Type genericParameter)
        {
            var key = Tuple.Create(genericType, genericParameter);

            if (false == cache.TryGetValue(key, out var creator))
            {
                var type = genericType.MakeGenericType(genericParameter);

                creator = Expression.Lambda<Func<object>>(Expression.New(type)).Compile();

                cache[key] = creator;
            }

            return creator.Invoke();
        }
    }
}