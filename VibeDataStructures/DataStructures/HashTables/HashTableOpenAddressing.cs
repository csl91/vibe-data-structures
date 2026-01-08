using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.HashTables
{
    /// <summary>
    /// Hash table implementation using open addressing with linear probing
    /// 
    /// Time Complexity:
    /// - Insert: O(1) average, O(n) worst case (high load factor)
    /// - Find: O(1) average, O(n) worst case
    /// - Remove: O(1) average, O(n) worst case
    /// 
    /// Space Complexity: O(n) - more cache-friendly than chaining
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class HashTableOpenAddressing<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private enum SlotState
        {
            Empty,
            Occupied,
            Deleted
        }

        private class HashSlot
        {
            public T? Data { get; set; }
            public SlotState State { get; set; }

            public HashSlot()
            {
                Data = default(T);
                State = SlotState.Empty;
            }
        }

        private HashSlot[] _slots;
        private int _count;
        private int _capacity;
        private const double LoadFactorThreshold = 0.7;

        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public int Capacity => _capacity;

        public HashTableOpenAddressing(int initialCapacity = 16)
        {
            _capacity = Math.Max(initialCapacity, 4);
            _slots = new HashSlot[_capacity];
            for (int i = 0; i < _capacity; i++)
            {
                _slots[i] = new HashSlot();
            }
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
        /// Finds the next available slot using linear probing
        /// </summary>
        private int FindSlot(T item, bool forInsertion = false)
        {
            int index = Hash(item);
            int originalIndex = index;

            do
            {
                var slot = _slots[index];
                
                if (slot.State == SlotState.Empty)
                    return forInsertion ? index : -1;
                
                if (slot.State == SlotState.Occupied && slot.Data!.CompareTo(item) == 0)
                    return index;
                
                if (slot.State == SlotState.Deleted && forInsertion)
                    return index;

                index = (index + 1) % _capacity;
            } while (index != originalIndex);

            return -1; // Table is full or item not found
        }

        /// <summary>
        /// Inserts an element using linear probing
        /// O(1) average case
        /// </summary>
        public bool Insert(T item)
        {
            if ((double)_count / _capacity >= LoadFactorThreshold)
            {
                Resize();
            }

            int index = FindSlot(item, forInsertion: true);
            if (index == -1)
                return false; // Table is full

            var slot = _slots[index];
            if (slot.State == SlotState.Occupied && slot.Data!.CompareTo(item) == 0)
                return false; // Duplicate not allowed

            slot.Data = item;
            slot.State = SlotState.Occupied;
            _count++;
            return true;
        }

        /// <summary>
        /// Finds an element using linear probing
        /// O(1) average case
        /// </summary>
        public bool Find(T item)
        {
            return FindSlot(item) != -1;
        }

        /// <summary>
        /// Removes an element using tombstone marking
        /// O(1) average case
        /// </summary>
        public bool Remove(T item)
        {
            int index = FindSlot(item);
            if (index == -1)
                return false;

            _slots[index].State = SlotState.Deleted;
            _slots[index].Data = default(T);
            _count--;
            return true;
        }

        private void Resize()
        {
            var oldSlots = _slots;
            _capacity *= 2;
            _slots = new HashSlot[_capacity];
            for (int i = 0; i < _capacity; i++)
            {
                _slots[i] = new HashSlot();
            }
            _count = 0;

            // Rehash all occupied elements
            foreach (var slot in oldSlots)
            {
                if (slot.State == SlotState.Occupied)
                {
                    Insert(slot.Data!);
                }
            }
        }

        public void Clear()
        {
            _slots = new HashSlot[_capacity];
            for (int i = 0; i < _capacity; i++)
            {
                _slots[i] = new HashSlot();
            }
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupTable = new HashTableOpenAddressing<T>();
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
                DataStructureName = "HashTableOpenAddressing",
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
                DataStructureName = "HashTableOpenAddressing",
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
                DataStructureName = "HashTableOpenAddressing",
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