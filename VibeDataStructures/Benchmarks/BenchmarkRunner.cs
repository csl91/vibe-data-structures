using System;
using System.Collections.Generic;
using System.Linq;
using VibeDataStructures.Interfaces;
using VibeDataStructures.DataStructures.Arrays;
using VibeDataStructures.DataStructures.LinkedLists;
using VibeDataStructures.DataStructures.Trees;
using VibeDataStructures.DataStructures.HashTables;
using VibeDataStructures.DataStructures.Heaps;
using VibeDataStructures.DataStructures.Stacks;

namespace VibeDataStructures.Benchmarks
{
    /// <summary>
    /// Comprehensive benchmark runner for comparing data structure performance
    /// </summary>
    public class BenchmarkRunner
    {
        private readonly List<(string Name, Func<IBenchmarkable<int>> Factory)> _dataStructures;

        public BenchmarkRunner()
        {
            _dataStructures = new List<(string, Func<IBenchmarkable<int>>)>
            {
                ("DynamicArray", () => new DynamicArray<int>()),
                ("StaticArray", () => new StaticArray<int>(10000)), // Large enough for most tests
                ("SinglyLinkedList", () => new SinglyLinkedList<int>()),
                ("DoublyLinkedList", () => new DoublyLinkedList<int>()),
                ("BinarySearchTree", () => new BinarySearchTree<int>()),
                ("HashTableChaining", () => new HashTableChaining<int>()),
                ("HashTableOpenAddressing", () => new HashTableOpenAddressing<int>()),
                ("BinaryMinHeap", () => new BinaryMinHeap<int>()),
                ("ArrayStack", () => new ArrayStack<int>()),
                ("CircularQueue", () => new CircularQueue<int>())
            };
        }

        /// <summary>
        /// Runs comprehensive benchmarks on all data structures
        /// </summary>
        public void RunComprehensiveBenchmark(int[]? testSizes = null, int iterations = 1)
        {
            testSizes ??= new[] { 100, 1000, 5000, 10000 };

            Console.WriteLine("=== Data Structure Performance Benchmark ===");
            Console.WriteLine($"Test Sizes: {string.Join(", ", testSizes)}");
            Console.WriteLine($"Iterations per test: {iterations}");
            Console.WriteLine();

            foreach (int size in testSizes)
            {
                Console.WriteLine($"\\n--- Testing with {size} elements ---");
                
                var testData = GenerateTestData(size);
                var shuffledData = GenerateShuffledTestData(size);

                // Sequential data benchmark
                Console.WriteLine("Sequential Data (1, 2, 3, ...):");
                RunBenchmarkForDataSet(testData, iterations);

                Console.WriteLine("\\nShuffled Data:");
                RunBenchmarkForDataSet(shuffledData, iterations);
            }
        }

        /// <summary>
        /// Runs benchmark for a specific data set
        /// </summary>
        private void RunBenchmarkForDataSet(int[] testData, int iterations)
        {
            var allResults = new List<PerformanceResult>();

            foreach (var (name, factory) in _dataStructures)
            {
                try
                {
                    Console.WriteLine($"\\nTesting {name}...");
                    
                    var avgResults = new Dictionary<string, (double avgMs, bool success)>();

                    // Run multiple iterations and average
                    for (int iter = 0; iter < iterations; iter++)
                    {
                        var dataStructure = factory();
                        var results = dataStructure.RunBenchmark(testData, warmupRuns: 2);
                        
                        foreach (var result in results)
                        {
                            if (!avgResults.ContainsKey(result.OperationName))
                                avgResults[result.OperationName] = (0, true);
                            
                            var current = avgResults[result.OperationName];
                            avgResults[result.OperationName] = (
                                current.avgMs + result.ElapsedMilliseconds,
                                current.success && result.Success
                            );
                        }
                    }

                    // Calculate averages and display
                    foreach (var kvp in avgResults)
                    {
                        var avgMs = kvp.Value.avgMs / iterations;
                        var success = kvp.Value.success;
                        
                        Console.WriteLine($"  {kvp.Key}: {avgMs:F4}ms (Success: {success})");
                        
                        allResults.Add(new PerformanceResult
                        {
                            DataStructureName = name,
                            OperationName = kvp.Key,
                            ElapsedMilliseconds = avgMs,
                            DataSize = testData.Length,
                            Success = success
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Error testing {name}: {ex.Message}");
                }
            }

            // Summary comparison
            Console.WriteLine("\\n--- Operation Summary ---");
            foreach (var operation in new[] { "Insert", "Find", "Remove" })
            {
                Console.WriteLine($"\\n{operation} Performance (fastest to slowest):");
                var operationResults = allResults
                    .Where(r => r.OperationName == operation && r.Success)
                    .OrderBy(r => r.ElapsedMilliseconds)
                    .ToList();

                for (int i = 0; i < operationResults.Count; i++)
                {
                    var result = operationResults[i];
                    var rank = i + 1;
                    Console.WriteLine($"  {rank}. {result.DataStructureName}: {result.ElapsedMilliseconds:F4}ms");
                }
            }
        }

        /// <summary>
        /// Generates sequential test data
        /// </summary>
        private int[] GenerateTestData(int size)
        {
            var data = new int[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = i + 1;
            }
            return data;
        }

        /// <summary>
        /// Generates shuffled test data to test worst-case scenarios
        /// </summary>
        private int[] GenerateShuffledTestData(int size)
        {
            var data = GenerateTestData(size);
            var random = new Random(42); // Fixed seed for reproducible results
            
            // Fisher-Yates shuffle
            for (int i = size - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (data[i], data[j]) = (data[j], data[i]);
            }
            
            return data;
        }

        /// <summary>
        /// Runs a focused comparison between specific data structures
        /// </summary>
        public void RunFocusedComparison(string[] dataStructureNames, int testSize = 1000)
        {
            Console.WriteLine($"=== Focused Comparison: {string.Join(" vs ", dataStructureNames)} ===");
            Console.WriteLine($"Test Size: {testSize} elements\\n");

            var testData = GenerateTestData(testSize);
            var selectedStructures = _dataStructures
                .Where(ds => dataStructureNames.Contains(ds.Name))
                .ToList();

            if (!selectedStructures.Any())
            {
                Console.WriteLine("No matching data structures found.");
                return;
            }

            foreach (var (name, factory) in selectedStructures)
            {
                try
                {
                    Console.WriteLine($"Testing {name}:");
                    var dataStructure = factory();
                    var results = dataStructure.RunBenchmark(testData);
                    
                    foreach (var result in results)
                    {
                        Console.WriteLine($"  {result}");
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Error: {ex.Message}\\n");
                }
            }
        }

        /// <summary>
        /// Lists all available data structures for benchmarking
        /// </summary>
        public void ListAvailableDataStructures()
        {
            Console.WriteLine("Available Data Structures:");
            foreach (var (name, _) in _dataStructures)
            {
                Console.WriteLine($"  - {name}");
            }
        }
    }
}