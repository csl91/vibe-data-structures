using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.Heaps
{
    /// <summary>
    /// Binary Min Heap implementation using array
    /// 
    /// Time Complexity:
    /// - Insert: O(log n) - bubble up
    /// - Find: O(1) for minimum, O(n) for arbitrary element
    /// - Remove: O(log n) - bubble down
    /// 
    /// Space Complexity: O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class BinaryMinHeap<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private T[] _heap;
        private int _count;
        private int _capacity;
        private const int DefaultCapacity = 16;

        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public int Capacity => _capacity;

        public BinaryMinHeap(int initialCapacity = DefaultCapacity)
        {
            _capacity = Math.Max(initialCapacity, DefaultCapacity);
            _heap = new T[_capacity];
            _count = 0;
        }

        /// <summary>
        /// Gets the parent index for a given index
        /// </summary>
        private int GetParentIndex(int index) => (index - 1) / 2;

        /// <summary>
        /// Gets the left child index for a given index
        /// </summary>
        private int GetLeftChildIndex(int index) => 2 * index + 1;

        /// <summary>
        /// Gets the right child index for a given index
        /// </summary>
        private int GetRightChildIndex(int index) => 2 * index + 2;

        /// <summary>
        /// Inserts an element and maintains heap property
        /// O(log n) time complexity
        /// </summary>
        public bool Insert(T item)
        {
            if (_count == _capacity)
            {
                Resize();
            }

            _heap[_count] = item;
            _count++;
            BubbleUp(_count - 1);
            return true;
        }

        /// <summary>
        /// Finds an element in the heap (linear search since no order guarantee)
        /// O(n) time complexity for arbitrary elements, O(1) for minimum
        /// </summary>
        public bool Find(T item)
        {
            if (_count == 0)
                return false;

            // Quick check for minimum element
            if (_heap[0].CompareTo(item) == 0)
                return true;

            // Linear search for other elements
            for (int i = 0; i < _count; i++)
            {
                if (_heap[i].CompareTo(item) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes an element from the heap
        /// O(n) to find + O(log n) to maintain heap property
        /// </summary>
        public bool Remove(T item)
        {
            // Find the element
            int indexToRemove = -1;
            for (int i = 0; i < _count; i++)
            {
                if (_heap[i].CompareTo(item) == 0)
                {
                    indexToRemove = i;
                    break;
                }
            }

            if (indexToRemove == -1)
                return false;

            // Replace with last element
            _heap[indexToRemove] = _heap[_count - 1];
            _count--;

            if (_count > 0 && indexToRemove < _count)
            {
                // Maintain heap property
                BubbleUp(indexToRemove);
                BubbleDown(indexToRemove);
            }

            return true;
        }

        /// <summary>
        /// Removes and returns the minimum element
        /// O(log n) time complexity
        /// </summary>
        public T ExtractMin()
        {
            if (_count == 0)
                throw new InvalidOperationException("Heap is empty");

            T min = _heap[0];
            _heap[0] = _heap[_count - 1];
            _count--;
            BubbleDown(0);
            return min;
        }

        /// <summary>
        /// Peeks at the minimum element without removing it
        /// O(1) time complexity
        /// </summary>
        public T PeekMin()
        {
            if (_count == 0)
                throw new InvalidOperationException("Heap is empty");
            return _heap[0];
        }

        private void BubbleUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = GetParentIndex(index);
                if (_heap[index].CompareTo(_heap[parentIndex]) >= 0)
                    break;

                Swap(index, parentIndex);
                index = parentIndex;
            }
        }

        private void BubbleDown(int index)
        {
            while (true)
            {
                int leftChild = GetLeftChildIndex(index);
                int rightChild = GetRightChildIndex(index);
                int smallest = index;

                if (leftChild < _count && _heap[leftChild].CompareTo(_heap[smallest]) < 0)
                    smallest = leftChild;

                if (rightChild < _count && _heap[rightChild].CompareTo(_heap[smallest]) < 0)
                    smallest = rightChild;

                if (smallest == index)
                    break;

                Swap(index, smallest);
                index = smallest;
            }
        }

        private void Swap(int i, int j)
        {
            T temp = _heap[i];
            _heap[i] = _heap[j];
            _heap[j] = temp;
        }

        private void Resize()
        {
            _capacity *= 2;
            T[] newHeap = new T[_capacity];
            Array.Copy(_heap, newHeap, _count);
            _heap = newHeap;
        }

        public void Clear()
        {
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupHeap = new BinaryMinHeap<T>();
                foreach (var item in testData)
                    warmupHeap.Insert(item);
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
                DataStructureName = "BinaryMinHeap",
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
                DataStructureName = "BinaryMinHeap",
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
                DataStructureName = "BinaryMinHeap",
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