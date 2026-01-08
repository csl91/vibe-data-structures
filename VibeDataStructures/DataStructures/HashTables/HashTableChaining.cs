using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.HashTables
{
    /// <summary>
    /// Hash table implementation using chaining for collision resolution
    /// 
    /// Time Complexity:
    /// - Insert: O(1) average, O(n) worst case (all keys hash to same bucket)
    /// - Find: O(1) average, O(n) worst case
    /// - Remove: O(1) average, O(n) worst case
    /// 
    /// Space Complexity: O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class HashTableChaining<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private class HashNode
        {
            public T Data { get; set; }
            public HashNode? Next { get; set; }

            public HashNode(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private HashNode?[] _buckets;
        private int _count;
        private int _capacity;
        private const double LoadFactorThreshold = 0.75;

        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public int Capacity => _capacity;

        public HashTableChaining(int initialCapacity = 16)
        {
            _capacity = Math.Max(initialCapacity, 4);
            _buckets = new HashNode[_capacity];
            _count = 0;
        }

        /// <summary>
        /// Hash function - simple modulo hash
        /// </summary>
        private int Hash(T item)
        {
            return Math.Abs(item.GetHashCode()) % _capacity;
        }

        /// <summary>
        /// Inserts an element using chaining
        /// O(1) average case
        /// </summary>
        public bool Insert(T item)
        {
            if ((double)_count / _capacity >= LoadFactorThreshold)
            {
                Resize();
            }

            int index = Hash(item);
            var current = _buckets[index];

            // Check if item already exists
            while (current != null)
            {
                if (current.Data.CompareTo(item) == 0)
                    return false; // Duplicate not allowed
                current = current.Next;
            }

            // Insert at the beginning of the chain
            var newNode = new HashNode(item)
            {
                Next = _buckets[index]
            };
            _buckets[index] = newNode;
            _count++;
            return true;
        }

        /// <summary>
        /// Finds an element in the hash table
        /// O(1) average case
        /// </summary>
        public bool Find(T item)
        {
            int index = Hash(item);
            var current = _buckets[index];

            while (current != null)
            {
                if (current.Data.CompareTo(item) == 0)
                    return true;
                current = current.Next;
            }

            return false;
        }

        /// <summary>
        /// Removes an element from the hash table
        /// O(1) average case
        /// </summary>
        public bool Remove(T item)
        {
            int index = Hash(item);
            var current = _buckets[index];
            HashNode? previous = null;

            while (current != null)
            {
                if (current.Data.CompareTo(item) == 0)
                {
                    if (previous == null)
                        _buckets[index] = current.Next;
                    else
                        previous.Next = current.Next;
                    
                    _count--;
                    return true;
                }
                previous = current;
                current = current.Next;
            }

            return false;
        }

        private void Resize()
        {
            var oldBuckets = _buckets;
            _capacity *= 2;
            _buckets = new HashNode[_capacity];
            _count = 0;

            // Rehash all elements
            for (int i = 0; i < oldBuckets.Length; i++)
            {
                var current = oldBuckets[i];
                while (current != null)
                {
                    Insert(current.Data);
                    current = current.Next;
                }
            }
        }

        public void Clear()
        {
            _buckets = new HashNode[_capacity];
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupTable = new HashTableChaining<T>();
                foreach (var item in testData)
                    warmupTable.Insert(item);
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
                DataStructureName = "HashTableChaining",
                OperationName = "Insert",
                ElapsedTicks = sw.ElapsedTicks,
                ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
                DataSize = testData.Length,
                Success = insertedCount > 0
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
                DataStructureName = "HashTableChaining",
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
                DataStructureName = "HashTableChaining",
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