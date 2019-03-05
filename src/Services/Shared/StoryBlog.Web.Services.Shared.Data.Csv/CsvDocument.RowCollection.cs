using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public partial class CsvDocument
    {
        internal class RowCollection : CollectionBase, IList<CsvRow>
        {
            private readonly CsvDocument document;
            private int version;

            public bool IsReadOnly => false;

            public CsvRow this[int index]
            {
                get
                {
                    if (0 > index && index >= Count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    return (CsvRow) InnerList[index];
                }
                set
                {
                    if (0 > index && index > Count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    InnerList[index] = value;
                    Interlocked.Increment(ref version);
                }
            }

            internal RowCollection(CsvDocument document)
            {
                this.document = document;
            }

            public new IEnumerator<CsvRow> GetEnumerator() => new Iterator(this);

            public void Add(CsvRow item)
            {
                if (null == item)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                if (InnerList.Contains(item))
                {
                    throw new InvalidOperationException();
                }

                InnerList.Add(item);
                Interlocked.Increment(ref version);
            }

            public bool Contains(CsvRow item)
            {
                if (null == item)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                return InnerList.Contains(item);
            }

            public void CopyTo(CsvRow[] array, int arrayIndex)
            {
                InnerList.CopyTo(array, arrayIndex);
            }

            public bool Remove(CsvRow item)
            {
                if (null == item)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                var index = InnerList.IndexOf(item);

                if (0 > index)
                {
                    return false;
                }

                InnerList.RemoveAt(index);
                Interlocked.Increment(ref version);

                return true;
            }

            public int IndexOf(CsvRow item)
            {
                if (null == item)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                return InnerList.IndexOf(item);
            }

            public void Insert(int index, CsvRow item)
            {
                if (null == item)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                if (0 > index)
                {
                    throw new IndexOutOfRangeException();
                }

                InnerList.Insert(index, item);
                Interlocked.Increment(ref version);
            }

            /// <summary>
            /// 
            /// </summary>
            private class Iterator : IEnumerator<CsvRow>
            {
                private readonly RowCollection collection;
                private readonly int version;
                private bool disposed;
                private int index;

                public CsvRow Current
                {
                    get
                    {
                        EnsureNotDisposed();
                        EnsureInitialized();

                        return (CsvRow) collection.InnerList[index];
                    }
                }

                object IEnumerator.Current => Current;

                public Iterator(RowCollection collection)
                {
                    this.collection = collection;
                    version = collection.version;
                    index = -1;
                }

                public bool MoveNext()
                {
                    EnsureNotDisposed();
                    EnsureNotModified();

                    if (-1 == index)
                    {
                        index = 0 < collection.Count ? 0 : -1;
                    }
                    else if (collection.Count > index)
                    {
                        index++;
                    }

                    return -1 < index && index < collection.Count;
                }

                public void Reset()
                {
                    EnsureNotDisposed();
                    EnsureNotModified();

                    index = 0 < collection.Count ? 0 : -1;
                }

                public void Dispose()
                {
                    Dispose(true);
                }

                private void Dispose(bool dispose)
                {
                    if (disposed)
                    {
                        return;
                    }

                    try
                    {
                        if (dispose)
                        {
                            
                        }
                    }
                    finally
                    {
                        disposed = true;
                    }
                }

                private void EnsureNotDisposed()
                {
                    if (disposed)
                    {
                        throw new ObjectDisposedException(GetType().Name);
                    }
                }

                private void EnsureInitialized()
                {
                    if (-1 == index)
                    {
                        throw new InvalidOperationException();
                    }
                }

                private void EnsureNotModified()
                {
                    if (version != collection.version)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }
    }
}