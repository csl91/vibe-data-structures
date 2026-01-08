using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.LinkedLists
{
    /// <summary>
    /// Singly linked list implementation
    /// 
    /// Time Complexity:
    /// - Insert: O(1) at head, O(n) at specific position
    /// - Find: O(n) - linear search
    /// - Remove: O(n) - need to find element first
    /// 
    /// Space Complexity: O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class SinglyLinkedList<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node? Next { get; set; }

            public Node(T data)
            {
                Data = data;
                Next = null;
            }
        }

        private Node? _head;
        private int _count;

        public int Count => _count;
        public bool IsEmpty => _head == null;

        public SinglyLinkedList()
        {
            _head = null;
            _count = 0;
        }

        /// <summary>
        /// Inserts an element at the head of the list
        /// O(1) time complexity
        /// </summary>
        public bool Insert(T item)
        {
            var newNode = new Node(item)
            {
                Next = _head
            };
            _head = newNode;
            _count++;
            return true;
        }

        /// <summary>
        /// Finds an element using linear search
        /// O(n) time complexity
        /// </summary>
        public bool Find(T item)
        {
            var current = _head;
            while (current != null)
            {
                if (current.Data.CompareTo(item) == 0)
                    return true;
                current = current.Next;
            }
            return false;
        }

        /// <summary>
        /// Removes the first occurrence of an element
        /// O(n) time complexity
        /// </summary>
        public bool Remove(T item)
        {
            if (_head == null)
                return false;

            // If head node contains the item
            if (_head.Data.CompareTo(item) == 0)
            {
                _head = _head.Next;
                _count--;
                return true;
            }

            // Search for the item in the rest of the list
            var current = _head;
            while (current.Next != null)
            {
                if (current.Next.Data.CompareTo(item) == 0)
                {
                    current.Next = current.Next.Next;
                    _count--;
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        public void Clear()
        {
            _head = null;
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupList = new SinglyLinkedList<T>();
                foreach (var item in testData)
                    warmupList.Insert(item);
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
                DataStructureName = "SinglyLinkedList",
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
                DataStructureName = "SinglyLinkedList",
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
                DataStructureName = "SinglyLinkedList",
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