using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Reactive
{
    internal sealed class PriorityQueue<T> where T : IComparable<T>
    {
        private long count = long.MinValue;
        private IndexedItem[] items;
        private int size;

        public int Count => size;

        public PriorityQueue()
            : this(16)
        {
        }

        public PriorityQueue(int capacity)
        {
            items = new IndexedItem[capacity];
            size = 0;
        }

        public T Peek()
        {
            if (0 == size)
            {
                throw new InvalidOperationException();
            }

            return items[0].Value;
        }

        public T Dequeue()
        {
            var result = Peek();
            RemoveAt(0);
            return result;
        }

        public void Enqueue(T item)
        {
            if (size >= items.Length)
            {
                var temp = items;

                items = new IndexedItem[items.Length * 2];

                Array.Copy(temp, items, temp.Length);
            }

            var index = size++;

            items[index] = new IndexedItem
            {
                Id = ++count,
                Value = item
            };

            Percolate(index);
        }

        private bool IsHigherPriority(int left, int right)
        {
            return items[left].CompareTo(items[right]) < 0;
        }

        private int Percolate(int index)
        {
            if (index >= size || index < 0)
            {
                return index;
            }

            var parent = (index - 1) / 2;
            while (parent >= 0 && parent != index && IsHigherPriority(index, parent))
            {
                // swap index and parent
                var temp = items[index];
                items[index] = items[parent];
                items[parent] = temp;

                index = parent;
                parent = (index - 1) / 2;
            }

            return index;
        }

        private void Heapify(int index)
        {
            if (index >= size || index < 0)
            {
                return;
            }

            while (true)
            {
                var left = 2 * index + 1;
                var right = 2 * index + 2;
                var first = index;

                if (left < size && IsHigherPriority(left, first))
                {
                    first = left;
                }

                if (right < size && IsHigherPriority(right, first))
                {
                    first = right;
                }

                if (first == index)
                {
                    break;
                }

                // swap index and first
                var temp = items[index];
                items[index] = items[first];
                items[first] = temp;

                index = first;
            }
        }

        public bool Remove(T item)
        {
            var comparer = EqualityComparer<T>.Default;

            for (var index = 0; index < size; ++index)
            {
                if (false == comparer.Equals(items[index].Value, item))
                {
                    continue;
                }

                RemoveAt(index);

                return true;
            }

            return false;
        }

        private void RemoveAt(int index)
        {
            items[index] = items[--size];
            items[size] = default;

            if (index == Percolate(index))
            {
                Heapify(index);
            }

            if (size < items.Length / 4)
            {
                var temp = items;
                items = new IndexedItem[items.Length / 2];
                Array.Copy(temp, 0, items, 0, size);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private struct IndexedItem : IComparable<IndexedItem>
        {
            public T Value;
            public long Id;

            public int CompareTo(IndexedItem other)
            {
                var c = Value.CompareTo(other.Value);

                if (0 == c)
                {
                    c = Id.CompareTo(other.Id);
                }

                return c;
            }
        }
    }
}