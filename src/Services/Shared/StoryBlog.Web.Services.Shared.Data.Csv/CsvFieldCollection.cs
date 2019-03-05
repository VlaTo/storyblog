using System;
using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public abstract class CsvFieldCollection : IEnumerable<CsvField>
    {
        protected readonly IList<CsvField> FieldsList;

        public CsvField this[int index]
        {
            get
            {
                if (0 > index || index >= FieldsList.Count)
                {
                    throw new ArgumentException("", nameof(index));
                }

                return FieldsList[index];
            }
        }

        protected CsvFieldCollection()
        {
            FieldsList = new List<CsvField>();
        }

        public abstract void Add(CsvField field);

        public abstract void Clear();

        public abstract int FindIndex(CsvField field);

        public IEnumerator<CsvField> GetEnumerator()
        {
            return FieldsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}