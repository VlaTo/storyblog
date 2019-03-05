using System;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public static class CsvFieldExtensions
    {
        public static TValue ReadAs<TValue>(this CsvField field)
        {
            if (null == field)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var value = field.Text;

            return (TValue) Convert.ChangeType(value, typeof(TValue));
        }
    }
}