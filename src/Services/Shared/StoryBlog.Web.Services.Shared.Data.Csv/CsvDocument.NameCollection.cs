using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public sealed partial class CsvDocument
    {
        internal class NameCollection : CollectionBase, ICollection<string>
        {
            private readonly CsvDocument document;
            private readonly StringComparer comparer;
            private int version;

            public bool IsReadOnly => false;

            public string this[int index]
            {
                get
                {
                    if (0 > index || index >= InnerList.Count)
                    {
                        throw new ArgumentException("", nameof(index));
                    }

                    return (string) InnerList[index];
                }
            }

            internal NameCollection(CsvDocument document)
                : this()
            {
                this.document = document;
            }

            private NameCollection()
            {
                comparer = StringComparer.InvariantCultureIgnoreCase;
            }

            public new IEnumerator<string> GetEnumerator() => new Iterator(this);

            public void Add(string item)
            {
                if (null == item)
                {
                    throw new ArgumentNullException(nameof(item));
                }

                if (String.IsNullOrWhiteSpace(item))
                {
                    throw new ArgumentException("", nameof(item));
                }

                if (Contains(item))
                {
                    throw new ArgumentException("", nameof(item));
                }

                InnerList.Add(item);
                Interlocked.Increment(ref version);
            }

            public bool Contains(string item) => -1 < InnerList.BinarySearch(0, InnerList.Count, item, comparer);

            public void CopyTo(string[] array, int arrayIndex)
            {
                InnerList.CopyTo(array, arrayIndex);
            }

            public bool Remove(string item)
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

            /// <summary>
            /// 
            /// </summary>
            private class Iterator : IEnumerator<string>
            {
                private readonly NameCollection collection;
                private readonly int version;
                private bool disposed;
                private int index;

                public string Current
                {
                    get
                    {
                        EnsureNotDisposed();
                        EnsureInitialized();
                        EnsureNotModified();

                        return (string) collection.InnerList[index];
                    }
                }

                object IEnumerator.Current => Current;

                public Iterator(NameCollection collection)
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

                private void EnsureNotDisposed()
                {
                    if (disposed)
                    {
                        throw new ObjectDisposedException(nameof(Iterator));
                    }
                }

                private void EnsureNotModified()
                {
                    if (version != collection.version)
                    {
                        throw new InvalidOperationException();
                    }
                }

                private void EnsureInitialized()
                {
                    if (-1 == index)
                    {
                        throw new InvalidOperationException();
                    }
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
            }
        }
    }
}