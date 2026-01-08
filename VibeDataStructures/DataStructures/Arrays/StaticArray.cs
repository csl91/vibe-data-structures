using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.Arrays
{
    /// <summary>
    /// Static array implementation with fixed capacity
    /// 
    /// Time Complexity:
    /// - Insert: O(1) if space available, O(n) when full (need to shift or reject)
    /// - Find: O(n) - linear search
    /// - Remove: O(n) - need to shift elements
    /// 
    /// Space Complexity: O(n) - fixed allocation
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class StaticArray<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private readonly T[] _items;
        private int _count;
        private readonly int _capacity;

        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public bool IsFull => _count == _capacity;
        public int Capacity => _capacity;

        public StaticArray(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be positive", nameof(capacity));

            _capacity = capacity;
            _items = new T[_capacity];
            _count = 0;
        }

        /// <summary>
        /// Inserts an element if space is available
        /// O(1) time complexity
        /// </summary>
        public bool Insert(T item)
        {
            if (IsFull)
                return false; // Cannot insert, array is full

            _items[_count++] = item;
            return true;
        }

        /// <summary>
        /// Inserts an element at a specific index
        /// O(n) time complexity due to shifting
        /// </summary>
        public bool InsertAt(int index, T item)
        {
            if (index < 0 || index > _count || IsFull)
                return false;

            // Shift elements to the right
            for (int i = _count; i > index; i--)
            {
                _items[i] = _items[i - 1];
            }

            _items[index] = item;
            _count++;
            return true;
        }

        /// <summary>
        /// Finds an element using linear search
        /// O(n) time complexity
        /// </summary>
        public bool Find(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].CompareTo(item) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the index of an element
        /// O(n) time complexity
        /// </summary>
        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].CompareTo(item) == 0)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Gets an element at a specific index
        /// O(1) time complexity
        /// </summary>
        public T GetAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();
            return _items[index];
        }

        /// <summary>
        /// Sets an element at a specific index
        /// O(1) time complexity
        /// </summary>
        public bool SetAt(int index, T item)
        {
            if (index < 0 || index >= _count)
                return false;
            _items[index] = item;
            return true;
        }

        /// <summary>
        /// Removes the first occurrence of an element
        /// O(n) time complexity due to shifting
        /// </summary>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;

            return RemoveAt(index);
        }

        /// <summary>
        /// Removes an element at a specific index
        /// O(n) time complexity due to shifting
        /// </summary>
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                return false;

            // Shift elements to the left
            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }

            _count--;
            _items[_count] = default(T)!; // Clear the last element
            return true;
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                _items[i] = default(T)!;
            }
            _count = 0;
        }

        /// <summary>
        /// Converts the array to a regular array
        /// </summary>
        public T[] ToArray()
        {
            T[] result = new T[_count];
            Array.Copy(_items, result, _count);
            return result;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Note: For static array, we need to ensure capacity is sufficient
            int requiredCapacity = testData.Length;
            if (_capacity < requiredCapacity)
            {
                throw new InvalidOperationException($"Array capacity ({_capacity}) is insufficient for test data size ({requiredCapacity})");
            }

            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupArray = new StaticArray<T>(requiredCapacity);
                foreach (var item in testData)
                    warmupArray.Insert(item);
            }

            // Benchmark Insert
            Clear();
            var sw = Stopwatch.StartNew();
            int insertedCount = 0;
            foreach (var item in testData)
            {
                if (Insert(item))
                    insertedCount++;
            }
            sw.Stop();
            
            results.Add(new PerformanceResult
            {
                DataStructureName = "StaticArray",
                OperationName = "Insert",
                ElapsedTicks = sw.ElapsedTicks,
                ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
                DataSize = testData.Length,
                Success = insertedCount == testData.Length
            });

            // Benchmark Find
            sw.Restart();
            bool allFound = true;
            foreach (var item in testData)
            {
                if (!Find(item))
                    allFound = false;
            }
            sw.Stop();

            results.Add(new PerformanceResult
            {
                DataStructureName = "StaticArray",
                OperationName = "Find",
                ElapsedTicks = sw.ElapsedTicks,
                ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
                DataSize = testData.Length,
                Success = allFound
            });

            // Benchmark Remove
            sw.Restart();
            foreach (var item in testData)
            {
                Remove(item);
            }
            sw.Stop();

            results.Add(new PerformanceResult
            {
                DataStructureName = "StaticArray",
                OperationName = "Remove",
                ElapsedTicks = sw.ElapsedTicks,
                ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
                DataSize = testData.Length,
                Success = IsEmpty
            });

            return results;
        }
    }
}