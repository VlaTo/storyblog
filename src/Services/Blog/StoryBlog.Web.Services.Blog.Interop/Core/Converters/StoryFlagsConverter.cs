using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using StoryBlog.Web.Services.Blog.Interop.Includes;

namespace StoryBlog.Web.Services.Blog.Interop.Core.Converters
{
    internal sealed class StoryFlagsConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(String) == sourceType)
            {
                return true;
            }

            return Enum.GetUnderlyingType(typeof(StoryFlags)) == sourceType || base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            // Authors|Comments
            if (null == value)
            {
                return base.ConvertFrom(context, culture, null);
            }

            if (value is string str)
            {
                if (Enums.TryParse(str, out StoryFlags result))
                {
                    return result;
                }

                throw new InvalidCastException();
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return base.CreateInstance(context, propertyValues);
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            //return base.GetCreateInstanceSupported(context);
            return true;
        }
    }
}