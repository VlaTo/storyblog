using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Blazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public enum PrefixSeparators
    {
        /// <summary>
        /// 
        /// </summary>
        Dash,

        /// <summary>
        /// 
        /// </summary>
        Element,

        /// <summary>
        /// 
        /// </summary>
        Modifier
    }

    public class ClassBuilder<TComponent>
        where TComponent : BootstrapComponentBase
    {
        private const char ClassNameSeparator = ' ';
        private const string DashSeparator = "-";
        private const string ElementSeparator = "_";
        private const string ModifierSeparator = "--";

        private readonly string classNamePrefix;
        private readonly StringBuilder builder;
        private readonly IList<ClassDefinition> classDefinitions;

        public ClassBuilder(string classNamePrefix, string componentPrefix)
        {
            this.classNamePrefix = classNamePrefix;

            builder = new StringBuilder();
            classDefinitions = new List<ClassDefinition>();

            if (false == String.IsNullOrWhiteSpace(componentPrefix))
            {
                if (false == String.IsNullOrWhiteSpace(classNamePrefix))
                {
                    this.classNamePrefix += (DashSeparator + componentPrefix);
                }
            }
        }

        public string Build(TComponent component, string extras = "", bool addClassNamePrefix = true)
        {
            if (0 == classDefinitions.Count)
            {
                return String.Empty;
            }

            builder.Clear();

            if (addClassNamePrefix)
            {
                builder.Append(classNamePrefix);
            }

            foreach (var definition in classDefinitions)
            {
                if (null == definition.Condition || definition.Condition.Invoke(component))
                {
                    if (0 < builder.Length)
                    {
                        builder.Append(ClassNameSeparator);
                    }

                    if (null == definition.ValueGetter)
                    {
                        builder.Append(definition.Name);
                        continue;
                    }

                    builder
                        .Append(definition.Name)
                        .Append(definition.PrefixSeparator)
                        .Append(definition.ValueGetter.Invoke(component));
                }
            }

            if (false == String.IsNullOrWhiteSpace(extras))
            {
                builder.Append(ClassNameSeparator).Append(extras);
            }

            return builder.ToString();
        }

        public ClassBuilder<TComponent> DefineClass(
            Func<TComponent, string> valueGetter,
            PrefixSeparators prefixSeparator = PrefixSeparators.Dash)
        {
            if (null == valueGetter)
            {
                throw new ArgumentNullException(nameof(valueGetter));
            }

            classDefinitions.Add(new ClassDefinition(
                classNamePrefix,
                valueGetter,
                _ => true,
                GetSeparator(prefixSeparator))
            );

            return this;
        }

        public ClassBuilder<TComponent> DefineClass(
            Func<TComponent, string> valueGetter,
            Predicate<TComponent> condition = default(Predicate<TComponent>),
            PrefixSeparators prefixSeparator = PrefixSeparators.Dash)
        {
            if (null == valueGetter)
            {
                throw new ArgumentNullException(nameof(valueGetter));
            }

            classDefinitions.Add(new ClassDefinition(
                classNamePrefix,
                valueGetter,
                condition,
                GetSeparator(prefixSeparator))
            );

            return this;
        }

        public ClassBuilder<TComponent> DefineClass(
            string value,
            Predicate<TComponent> condition = default(Predicate<TComponent>),
            PrefixSeparators prefixSeparator = PrefixSeparators.Dash)
        {
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("", nameof(value));
            }

            var prefix = new StringBuilder()
                .Append(classNamePrefix)
                .Append(GetSeparator(prefixSeparator))
                .Append(value);

            classDefinitions.Add(new ClassDefinition(
                prefix.ToString(),
                default(Func<TComponent, string>),
                condition,
                String.Empty)
            );

            return this;
        }

        private static string GetSeparator(PrefixSeparators prefix)
        {
            switch (prefix)
            {
                case PrefixSeparators.Dash:
                {
                    return DashSeparator;
                }

                case PrefixSeparators.Element:
                {
                    return ElementSeparator;
                }

                case PrefixSeparators.Modifier:
                {
                    return ModifierSeparator;
                }

                default:
                {
                    return DashSeparator;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class ClassDefinition
        {
            public string Name
            {
                get;
            }

            public Func<TComponent, string> ValueGetter
            {
                get;
            }

            public Predicate<TComponent> Condition
            {
                get;
            }

            public string PrefixSeparator
            {
                get;
            }

            public ClassDefinition(string name, Func<TComponent, string> valueGetter, Predicate<TComponent> condition, string prefixSeparator)
            {
                Name = name;
                ValueGetter = valueGetter;
                Condition = condition;
                PrefixSeparator = prefixSeparator;
            }
        }
    }
}