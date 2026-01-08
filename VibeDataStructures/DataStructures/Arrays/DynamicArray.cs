using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.Arrays
{
    /// <summary>
    /// Dynamic array implementation with resizable capacity
    /// Similar to .NET's List<T> but with explicit resize logic for benchmarking
    /// 
    /// Time Complexity:
    /// - Insert: O(1) amortized, O(n) worst case (when resizing)
    /// - Find: O(n) - linear search
    /// - Remove: O(n) - need to shift elements
    /// 
    /// Space Complexity: O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class DynamicArray<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 4;
        private const double GrowthFactor = 2.0;

        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public int Capacity => _items.Length;

        public DynamicArray(int initialCapacity = DefaultCapacity)
        {
            if (initialCapacity < 0)
                throw new ArgumentOutOfRangeException(nameof(initialCapacity));

            _items = new T[Math.Max(initialCapacity, DefaultCapacity)];
            _count = 0;
        }

        /// <summary>
        /// Inserts an element at the end of the array
        /// O(1) amortized, O(n) worst case when resizing
        /// </summary>
        public bool Insert(T item)
        {
            if (_count == _items.Length)
            {
                Resize();
            }

            _items[_count++] = item;
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
        /// Removes the first occurrence of an element
        /// O(n) time complexity due to shifting elements
        /// </summary>
        public bool Remove(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_items[i].CompareTo(item) == 0)
                {
                    // Shift elements left
                    for (int j = i; j < _count - 1; j++)
                    {
                        _items[j] = _items[j + 1];
                    }
                    _count--;
                    _items[_count] = default(T)!; // Clear reference
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                _items[i] = default(T)!;
            }
            _count = 0;
        }

        private void Resize()
        {
            int newCapacity = (int)(_items.Length * GrowthFactor);
            T[] newItems = new T[newCapacity];
            Array.Copy(_items, newItems, _count);
            _items = newItems;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupArray = new DynamicArray<T>();
                foreach (var item in testData)
                    warmupArray.Insert(item);
            }

            // Benchmark Insert
            Clear();
            var sw = Stopwatch.StartNew();
            foreach (var item in testData)
            {
                Insert(item);
            }
            sw.Stop();
            
            results.Add(new PerformanceResult
            {
                DataStructureName = "DynamicArray",
                OperationName = "Insert",
                ElapsedTicks = sw.ElapsedTicks,
                ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
                DataSize = testData.Length,
                Success = Count == testData.Length
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
                DataStructureName = "DynamicArray",
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
                DataStructureName = "DynamicArray",
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