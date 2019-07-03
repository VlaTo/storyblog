using System;
using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Core
{
    public sealed class SortedList<T> : IList<T>, IReadOnlyCollection<T>
    {
        private readonly IComparer<T> comparer;
        private readonly ArrayList list;
        private int version;

        public int Count => list.Count;

        public bool IsReadOnly => list.IsReadOnly;
        
        public T this[int index]
        {
            get => (T) list[index];
            set => throw new NotSupportedException();
        }

        public SortedList(IComparer<T> comparer)
        {
            this.comparer = comparer;
            list = new ArrayList();
        }

        public IEnumerator<T> GetEnumerator() => new ItemEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (list.Contains(item))
            {
                return;
            }

            int FindPosition()
            {
                var position = 0;

                for (; position < list.Count; position++)
                {
                    var compare = comparer.Compare((T)list[position], item);

                    if (0 < compare)
                    {
                        break;
                    }
                }

                return position;
            }

            version++;

            var index = FindPosition();

            list.Insert(index, item);
        }

        public void Clear()
        {
            version++;
            list.Clear();
        }

        /// <summary>
        /// Adds the elements of the specified collection to the <see cref="SortedList{T}" />.
        /// </summary>
        /// <param name="collection">
        /// The collection whose elements should be added to the end of the <see cref="SortedList{T}" />.
        /// The collection itself cannot be <see langword="null" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="collection">collection</paramref> is <see langword="null" />.
        /// </exception>
        public void AddRange(IEnumerable<T> collection)
        {
            if (null == collection)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public bool Contains(T item) => list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = IndexOf(item);

            if (0 > index)
            {
                return false;
            }

            version++;
            list.RemoveAt(index);

            return true;
        }

        public int IndexOf(T item)
        {
            if (null == item)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            version++;
            list.RemoveAt(index);
        }

        /// <summary>
        /// 
        /// </summary>
        private class ItemEnumerator : IEnumerator<T>
        {
            private readonly SortedList<T> sortedList;
            private readonly int version;
            private int index;
            private bool disposed;

            public T Current
            {
                get
                {
                    EnsureNotModified();

                    if (0 > index)
                    {
                        throw new InvalidOperationException();
                    }

                    if (index >= sortedList.Count)
                    {
                        throw new InvalidOperationException();
                    }

                    return sortedList[index];
                }
            }

            object IEnumerator.Current => Current;

            public ItemEnumerator(SortedList<T> sortedList)
            {
                this.sortedList = sortedList;
                version = sortedList.version;
                index = -1;
            }

            public bool MoveNext()
            {
                EnsureNotModified();

                if (-1 == index)
                {
                    if (0 < sortedList.Count)
                    {
                        index = 0;
                        return true;
                    }
                }

                var lastIndex = sortedList.Count - 1;

                if (index < lastIndex)
                {
                    index++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                EnsureNotModified();
                index = -1;
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

            private void EnsureNotModified()
            {
                if (version == sortedList.version)
                {
                    return;
                }

                throw new InvalidOperationException();
            }
        }
    }
}