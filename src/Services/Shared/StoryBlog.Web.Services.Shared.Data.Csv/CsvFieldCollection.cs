using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public abstract class CsvFieldCollection : IEnumerable<CsvField>
    {
        protected readonly IList<CsvField> FieldsList;

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