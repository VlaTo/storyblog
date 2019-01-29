using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public sealed partial class CsvDocument
    {
        private class NameCollection : CollectionBase, ICollection<string>
        {
            private readonly CsvDocument document;

            public bool IsReadOnly => false;

            public NameCollection(CsvDocument document)
            {
                this.document = document;
            }

            public new IEnumerator<string> GetEnumerator()
            {
                throw new System.NotImplementedException();
            }

            public void Add(string item)
            {
                throw new System.NotImplementedException();
            }

            public bool Contains(string item)
            {
                throw new System.NotImplementedException();
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                throw new System.NotImplementedException();
            }

            public bool Remove(string item)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}