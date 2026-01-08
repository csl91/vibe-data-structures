using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.Trees
{
    /// <summary>
    /// Binary Search Tree implementation (unbalanced)
    /// 
    /// Time Complexity:
    /// - Insert: O(log n) average, O(n) worst case (skewed tree)
    /// - Find: O(log n) average, O(n) worst case
    /// - Remove: O(log n) average, O(n) worst case
    /// 
    /// Space Complexity: O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class BinarySearchTree<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private class TreeNode
        {
            public T Data { get; set; }
            public TreeNode? Left { get; set; }
            public TreeNode? Right { get; set; }

            public TreeNode(T data)
            {
                Data = data;
                Left = null;
                Right = null;
            }
        }

        private TreeNode? _root;
        private int _count;

        public int Count => _count;
        public bool IsEmpty => _root == null;

        public BinarySearchTree()
        {
            _root = null;
            _count = 0;
        }

        /// <summary>
        /// Inserts an element maintaining BST property
        /// O(log n) average, O(n) worst case
        /// </summary>
        public bool Insert(T item)
        {
            if (_root == null)
            {
                _root = new TreeNode(item);
                _count++;
                return true;
            }

            return InsertRecursive(_root, item);
        }

        private bool InsertRecursive(TreeNode node, T item)
        {
            int comparison = item.CompareTo(node.Data);

            if (comparison == 0)
            {
                // Duplicate values not allowed in this implementation
                return false;
            }
            else if (comparison < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new TreeNode(item);
                    _count++;
                    return true;
                }
                return InsertRecursive(node.Left, item);
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new TreeNode(item);
                    _count++;
                    return true;
                }
                return InsertRecursive(node.Right, item);
            }
        }

        /// <summary>
        /// Finds an element using BST search property
        /// O(log n) average, O(n) worst case
        /// </summary>
        public bool Find(T item)
        {
            return FindRecursive(_root, item);
        }

        private bool FindRecursive(TreeNode? node, T item)
        {
            if (node == null)
                return false;

            int comparison = item.CompareTo(node.Data);

            if (comparison == 0)
                return true;
            else if (comparison < 0)
                return FindRecursive(node.Left, item);
            else
                return FindRecursive(node.Right, item);
        }

        /// <summary>
        /// Removes an element maintaining BST property
        /// O(log n) average, O(n) worst case
        /// </summary>
        public bool Remove(T item)
        {
            var result = RemoveRecursive(_root, item);
            _root = result.node;
            if (result.removed)
                _count--;
            return result.removed;
        }

        private (TreeNode? node, bool removed) RemoveRecursive(TreeNode? node, T item)
        {
            if (node == null)
                return (null, false);

            int comparison = item.CompareTo(node.Data);

            if (comparison < 0)
            {
                var result = RemoveRecursive(node.Left, item);
                node.Left = result.node;
                return (node, result.removed);
            }
            else if (comparison > 0)
            {
                var result = RemoveRecursive(node.Right, item);
                node.Right = result.node;
                return (node, result.removed);
            }
            else
            {
                // Node to be deleted found
                if (node.Left == null)
                    return (node.Right, true);
                if (node.Right == null)
                    return (node.Left, true);

                // Node with two children: get the inorder successor
                TreeNode successor = FindMinNode(node.Right);
                node.Data = successor.Data;
                var result = RemoveRecursive(node.Right, successor.Data);
                node.Right = result.node;
                return (node, true);
            }
        }

        private TreeNode FindMinNode(TreeNode node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        public void Clear()
        {
            _root = null;
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupTree = new BinarySearchTree<T>();
                foreach (var item in testData)
                    warmupTree.Insert(item);
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
                DataStructureName = "BinarySearchTree",
                OperationName = "Insert",
                ElapsedTicks = sw.ElapsedTicks,
                ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
                DataSize = testData.Length,
                Success = insertedCount > 0 // Some items might be duplicates
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
                DataStructureName = "BinarySearchTree",
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
                DataStructureName = "BinarySearchTree",
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