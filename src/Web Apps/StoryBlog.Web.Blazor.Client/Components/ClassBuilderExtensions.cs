using System;

namespace StoryBlog.Web.Blazor.Client.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class ClassBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IClassBuilder<TComponent> Name<TComponent>(this IClassBuilder<TComponent> builder, string value)
            where TComponent : BootstrapComponentBase
        {
            return builder.Name(_ => value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="func"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IClassBuilder<TComponent> Modifier<TComponent>(this IClassBuilder<TComponent> builder, Func<TComponent, string> func)
            where TComponent : BootstrapComponentBase
        {
            return builder.Modifier(func, _ => true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IClassBuilder<TComponent> Modifier<TComponent>(this IClassBuilder<TComponent> builder, string value)
            where TComponent : BootstrapComponentBase
        {
            return builder.Modifier(_ => value, _ => true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="builder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IClassBuilder<TComponent> Modifier<TComponent>(this IClassBuilder<TComponent> builder, string value, Predicate<TComponent> condition)
            where TComponent : BootstrapComponentBase
        {
            return builder.Modifier(_ => value, condition);
        }
    }
}