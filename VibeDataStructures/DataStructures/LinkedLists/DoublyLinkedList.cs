using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.LinkedLists
{
    /// <summary>
    /// Doubly linked list implementation with head and tail pointers
    /// 
    /// Time Complexity:
    /// - Insert: O(1) at head/tail, O(n) at specific position
    /// - Find: O(n) - linear search (can search from both ends)
    /// - Remove: O(n) to find, O(1) once found
    /// 
    /// Space Complexity: O(n) - extra pointer per node compared to singly linked
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class DoublyLinkedList<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node? Next { get; set; }
            public Node? Previous { get; set; }

            public Node(T data)
            {
                Data = data;
                Next = null;
                Previous = null;
            }
        }

        private Node? _head;
        private Node? _tail;
        private int _count;

        public int Count => _count;
        public bool IsEmpty => _head == null;

        public DoublyLinkedList()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        /// <summary>
        /// Inserts an element at the head of the list
        /// O(1) time complexity
        /// </summary>
        public bool Insert(T item)
        {
            var newNode = new Node(item);

            if (_head == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                newNode.Next = _head;
                _head.Previous = newNode;
                _head = newNode;
            }

            _count++;
            return true;
        }

        /// <summary>
        /// Inserts an element at the tail of the list
        /// O(1) time complexity
        /// </summary>
        public void InsertAtTail(T item)
        {
            var newNode = new Node(item);

            if (_tail == null)
            {
                _head = _tail = newNode;
            }
            else
            {
                _tail.Next = newNode;
                newNode.Previous = _tail;
                _tail = newNode;
            }

            _count++;
        }

        /// <summary>
        /// Finds an element using linear search
        /// O(n) time complexity - can optimize by searching from both ends
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
        /// O(n) time complexity to find, O(1) to remove
        /// </summary>
        public bool Remove(T item)
        {
            var current = _head;

            while (current != null)
            {
                if (current.Data.CompareTo(item) == 0)
                {
                    RemoveNode(current);
                    return true;
                }
                current = current.Next;
            }

            return false;
        }

        private void RemoveNode(Node node)
        {
            if (node.Previous != null)
                node.Previous.Next = node.Next;
            else
                _head = node.Next;

            if (node.Next != null)
                node.Next.Previous = node.Previous;
            else
                _tail = node.Previous;

            _count--;
        }

        public void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupList = new DoublyLinkedList<T>();
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
                DataStructureName = "DoublyLinkedList",
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
                DataStructureName = "DoublyLinkedList",
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
                DataStructureName = "DoublyLinkedList",
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