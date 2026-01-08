using VibeDataStructures.Benchmarks;
using VibeDataStructures.DataStructures.Arrays;
using VibeDataStructures.DataStructures.HashTables;
using VibeDataStructures.Tests;

namespace VibeDataStructures
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Vibe Data Structures Benchmarking ===");
            Console.WriteLine("Performance testing for Insert, Find, and Remove operations\\n");

            var benchmarkRunner = new BenchmarkRunner();

            if (args.Length == 0)
            {
                // Default: Run comprehensive benchmark
                Console.WriteLine("Running comprehensive benchmark...");
                benchmarkRunner.RunComprehensiveBenchmark(
                    testSizes: new[] { 100, 1000, 5000 }, 
                    iterations: 3
                );
            }
            else if (args[0] == "quick")
            {
                // Quick demo with smaller data sets
                Console.WriteLine("Running quick demo...");
                RunQuickDemo();
            }
            else if (args[0] == "test")
            {
                // Run basic correctness tests
                BasicTests.RunAllTests();
            }
            else if (args[0] == "compare" && args.Length >= 2)
            {
                // Compare specific data structures
                var structures = args.Skip(1).ToArray();
                benchmarkRunner.RunFocusedComparison(structures, testSize: 2000);
            }
            else if (args[0] == "list")
            {
                // List available data structures
                benchmarkRunner.ListAvailableDataStructures();
            }
            else
            {
                ShowUsage();
            }

            Console.WriteLine("\\nBenchmarking complete. Press any key to exit...");
            Console.ReadKey();
        }

        static void RunQuickDemo()
        {
            Console.WriteLine("Quick demonstration of data structure operations:\\n");

            // Demo Dynamic Array
            var dynamicArray = new DynamicArray<int>();
            Console.WriteLine("=== Dynamic Array Demo ===");
            for (int i = 1; i <= 5; i++)
                dynamicArray.Insert(i * 10);
            
            Console.WriteLine($"Inserted 5 elements. Count: {dynamicArray.Count}");
            Console.WriteLine($"Find 30: {dynamicArray.Find(30)}");
            Console.WriteLine($"Find 99: {dynamicArray.Find(99)}");
            Console.WriteLine($"Remove 30: {dynamicArray.Remove(30)}");
            Console.WriteLine($"Count after removal: {dynamicArray.Count}\\n");

            // Demo Hash Table
            var hashTable = new HashTableChaining<int>();
            Console.WriteLine("=== Hash Table Demo ===");
            for (int i = 1; i <= 5; i++)
                hashTable.Insert(i * 100);
            
            Console.WriteLine($"Inserted 5 elements. Count: {hashTable.Count}");
            Console.WriteLine($"Find 300: {hashTable.Find(300)}");
            Console.WriteLine($"Find 999: {hashTable.Find(999)}");
            Console.WriteLine($"Remove 300: {hashTable.Remove(300)}");
            Console.WriteLine($"Count after removal: {hashTable.Count}\\n");

            // Mini benchmark
            Console.WriteLine("=== Mini Benchmark (1000 elements) ===");
            var runner = new BenchmarkRunner();
            runner.RunFocusedComparison(
                new[] { "DynamicArray", "HashTableChaining", "BinarySearchTree" }, 
                testSize: 1000
            );
        }

        static void ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  VibeDataStructures                    # Run comprehensive benchmark");
            Console.WriteLine("  VibeDataStructures quick              # Run quick demo");
            Console.WriteLine("  VibeDataStructures test               # Run basic correctness tests");
            Console.WriteLine("  VibeDataStructures compare <ds1> <ds2> # Compare specific data structures");
            Console.WriteLine("  VibeDataStructures list               # List available data structures");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  VibeDataStructures compare DynamicArray HashTableChaining");
            Console.WriteLine("  VibeDataStructures compare BinarySearchTree HashTableOpenAddressing");
        }
    }
}
