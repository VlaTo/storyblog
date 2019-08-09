using StoryBlog.Web.Services.Blog.Interop.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace StoryBlog.Web.Services.Blog.Interop.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class Flags
    {
        /// <summary>
        /// Returns a Boolean telling whether a given integral value, or its name as a string, exists in a specified
        /// enumeration.
        /// </summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <param name="value">The value or name of a constant in <paramref name="enumType" />.</param>
        /// <returns><c>true</c> if a constant in <paramref name="enumType" /> has a value equal to <paramref name="value" />; otherwise, <c>false</c>.</returns>
        public static bool IsDefined(Type enumType, object value)
        {
            if (null == enumType)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (false == enumType.IsEnum)
            {
                throw new ArgumentException("", nameof(enumType));
            }

            if (null == value)
            {
                return false;
            }

            if (value is string str)
            {
                bool IsNameOrAliasDefined(string name)
                {
                    var comparer = StringComparer.InvariantCulture;
                    var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
                    var values = Enum.GetValues(enumType);

                    foreach (var item in values)
                    {
                        var x = Enum.GetName(enumType, item);

                        if (comparer.Equals(name, x))
                        {
                            return true;
                        }

                        var field = fields.First(property => comparer.Equals(property.Name, x));
                        var attribute = field.GetCustomAttribute<FlagAttribute>();

                        if (null == attribute)
                        {
                            continue;
                        }

                        if (comparer.Equals(attribute.Key, name))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                if (enumType.IsDefined(typeof(FlagsAttribute)))
                {
                    var separators = new[] {CultureInfo.InvariantCulture.TextInfo.ListSeparator};
                    var values = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    return Array.TrueForAll(values, IsNameOrAliasDefined);
                }

                return IsNameOrAliasDefined(str);
            }

            return Enum.IsDefined(enumType, value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an
        /// equivalent enumerated object.
        /// </summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>An object of type <paramref name="enumType"/> whose value is represented by <paramref name="value"/>.</returns>
        public static object Parse(Type enumType, string value)
        {
            if (null == enumType)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Parse(enumType, value, StringComparer.InvariantCulture);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an
        /// equivalent enumerated object. A parameter specifies whether the operation is case-insensitive.
        /// </summary>
        /// <param name="enumType">An enumeration type.</param>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <returns>An object of type <paramref name="enumType"/> whose value is represented by <paramref name="value"/>.</returns>
        public static object Parse(Type enumType, string value, bool ignoreCase)
        {
            if (null == enumType)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Parse(enumType, value, ignoreCase ? StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants
        /// to an equivalent enumerated object. The return value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value" />.</typeparam>
        /// <param name="value">The case-sensitive string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="result">
        /// When this method returns, <paramref name="result" /> contains an object of type <typeparamref name="TEnum" />
        /// whose value is represented by <paramref name="value" /> if the parse operation succeeds.
        /// If the parse operation fails, result contains the default value of the underlying type of <typeparamref name="TEnum" />.
        /// Note that this value need not be a member of the TEnum enumeration. This parameter is passed uninitialized.
        /// </param>
        /// <returns><c>true</c> if the value parameter was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParse<TEnum>(string value, out TEnum result)
            where TEnum : struct
        {
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var enumType = typeof(TEnum);
            var comparer = StringComparer.InvariantCulture;
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var enumValues = Enum.GetValues(enumType);

            string TranslateValue(string str)
            {
                foreach (var enumValue in enumValues)
                {
                    var name = Enum.GetName(enumType, enumValue);

                    if (comparer.Equals(name, str))
                    {
                        return name;
                    }

                    var field = fields.First(info => comparer.Equals(info.Name, name));
                    var attribute = field.GetCustomAttribute<FlagAttribute>();

                    if (null == attribute)
                    {
                        continue;
                    }

                    if (comparer.Equals(attribute.Key, str))
                    {
                        return name;
                    }
                }

                throw new KeyNotFoundException();
            }

            if (enumType.IsDefined(typeof(FlagsAttribute)))
            {
                var separator = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
                var values = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                var keys = Array.ConvertAll(values, TranslateValue);
                return Enum.TryParse(String.Join(separator, keys), out result);
            }

            return Enum.TryParse(TranslateValue(value), out result);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated constants to an
        /// equivalent enumerated object. A parameter specifies whether the operation is case-sensitive. The return
        /// value indicates whether the conversion succeeded.
        /// </summary>
        /// <typeparam name="TEnum">The enumeration type to which to convert <paramref name="value" />.</typeparam>
        /// <param name="value">The string representation of the enumeration name or underlying value to convert.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to consider case.</param>
        /// <param name="result">
        /// When this method returns, result contains an object of type <typeparamref name="TEnum" /> whose value
        /// is represented by <paramref name="value" /> if the parse operation succeeds.
        /// If the parse operation fails, result contains the default value of the underlying type of <typeparamref name="TEnum" />.
        /// Note that this value need not be a member of the <typeparamref name="TEnum" /> enumeration. This parameter
        /// is passed uninitialized.
        /// </param>
        /// <returns><c>true</c> if the value parameter was converted successfully; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentException"><typeparamref name="TEnum" /> is not an enumeration type.</exception>
        public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result)
            where TEnum : struct
        {
            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var enumType = typeof(TEnum);
            var comparer = ignoreCase ? StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture;
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var enumValues = Enum.GetValues(enumType);

            string TranslateValue(string str)
            {
                foreach (var enumValue in enumValues)
                {
                    var name = Enum.GetName(enumType, enumValue);

                    if (comparer.Equals(name, str))
                    {
                        return name;
                    }

                    var field = fields.First(info => comparer.Equals(info.Name, name));
                    var attribute = field.GetCustomAttribute<FlagAttribute>();

                    if (null == attribute)
                    {
                        continue;
                    }

                    if (comparer.Equals(attribute.Key, str))
                    {
                        return name;
                    }
                }

                throw new KeyNotFoundException();
            }

            if (enumType.IsDefined(typeof(FlagsAttribute)))
            {
                var separator = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
                var values = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                var keys = Array.ConvertAll(values, TranslateValue);
                return Enum.TryParse(String.Join(separator, keys), out result);
            }

            return Enum.TryParse(TranslateValue(value), out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(Type enumType, object value, string format)
        {
            if (null == enumType)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var separator = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
            var comparer = StringComparer.InvariantCulture;
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var names = value.ToString().Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            var values = Array.ConvertAll(names, x =>
            {
                var name = x.Trim();
                var property = fields.FirstOrDefault(field => comparer.Equals(field.Name, name));
                var attribute = property.GetCustomAttribute<FlagAttribute>();
                return null == attribute ? name : attribute.Key;
            });

            switch (format)
            {
                case "F":
                {
                    return String.Join(separator, values);
                }

                default:
                {
                    return String.Empty;
                }
            }
        }

        private static object Parse(Type enumType, string value, IEqualityComparer<string> comparer)
        {
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var enumValues = Enum.GetValues(enumType);

            string TranslateValue(string str)
            {
                foreach (var enumValue in enumValues)
                {
                    var name = Enum.GetName(enumType, enumValue);

                    if (comparer.Equals(name, str))
                    {
                        return name;
                    }

                    var field = fields.First(info => comparer.Equals(info.Name, name));
                    var attribute = field.GetCustomAttribute<FlagAttribute>();

                    if (null == attribute)
                    {
                        continue;
                    }

                    if (comparer.Equals(attribute.Key, str))
                    {
                        return name;
                    }
                }

                throw new KeyNotFoundException();
            }

            if (enumType.IsDefined(typeof(FlagsAttribute)))
            {
                var separator = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
                var values = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                var keys = Array.ConvertAll(values, TranslateValue);
                return Enum.Parse(enumType, String.Join(separator, keys));
            }

            return Enum.Parse(enumType, TranslateValue(value));
        }
    }
}