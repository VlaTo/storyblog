using System;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public partial class CsvRow
    {
        /// <summary>
        /// 
        /// </summary>
        private sealed class FieldCollection : CsvFieldCollection
        {
            private readonly CsvRow row;

            public FieldCollection(CsvRow row)
            {
                this.row = row;
            }

            public override void Add(CsvField field)
            {
                if (null == field)
                {
                    throw new ArgumentNullException(nameof(field));
                }

                if (0 <= FindIndex(field))
                {
                    throw new InvalidOperationException();
                }

                FieldsList.Add(field);
            }

            public override void Clear()
            {
                FieldsList.Clear();
            }

            public override int FindIndex(CsvField field)
            {
                if (null == field)
                {
                    throw new ArgumentNullException(nameof(field));
                }

                return FieldsList.IndexOf(field);
            }
        }
    }
}