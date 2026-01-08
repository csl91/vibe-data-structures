using System;
using VibeDataStructures.DataStructures.Arrays;
using VibeDataStructures.DataStructures.LinkedLists;
using VibeDataStructures.DataStructures.HashTables;

namespace VibeDataStructures.Tests
{
    /// <summary>
    /// Simple correctness tests for data structures
    /// Run these to verify basic functionality before performance testing
    /// </summary>
    public static class BasicTests
    {
        public static void RunAllTests()
        {
            Console.WriteLine("=== Running Basic Correctness Tests ===\\n");

            TestDynamicArray();
            TestSinglyLinkedList();
            TestHashTableChaining();
            
            Console.WriteLine("\\n=== All Basic Tests Completed ===");
        }

        private static void TestDynamicArray()
        {
            Console.WriteLine("Testing DynamicArray...");
            var arr = new DynamicArray<int>();
            
            // Test insertions
            for (int i = 1; i <= 10; i++)
                arr.Insert(i);
            Assert(arr.Count == 10, "Count should be 10 after 10 insertions");
            
            // Test finds
            Assert(arr.Find(5), "Should find element 5");
            Assert(!arr.Find(99), "Should not find element 99");
            
            // Test removals
            Assert(arr.Remove(5), "Should remove element 5");
            Assert(!arr.Find(5), "Should not find element 5 after removal");
            Assert(arr.Count == 9, "Count should be 9 after one removal");
            
            Console.WriteLine("  ✓ DynamicArray tests passed");
        }

        private static void TestSinglyLinkedList()
        {
            Console.WriteLine("Testing SinglyLinkedList...");
            var list = new SinglyLinkedList<int>();
            
            // Test insertions
            for (int i = 1; i <= 5; i++)
                list.Insert(i);
            Assert(list.Count == 5, "Count should be 5 after 5 insertions");
            
            // Test finds
            Assert(list.Find(3), "Should find element 3");
            Assert(!list.Find(99), "Should not find element 99");
            
            // Test removals
            Assert(list.Remove(3), "Should remove element 3");
            Assert(!list.Find(3), "Should not find element 3 after removal");
            Assert(list.Count == 4, "Count should be 4 after one removal");
            
            Console.WriteLine("  ✓ SinglyLinkedList tests passed");
        }

        private static void TestHashTableChaining()
        {
            Console.WriteLine("Testing HashTableChaining...");
            var hash = new HashTableChaining<int>();
            
            // Test insertions
            for (int i = 1; i <= 20; i++)
                hash.Insert(i);
            Assert(hash.Count == 20, "Count should be 20 after 20 insertions");
            
            // Test duplicate insertion
            Assert(!hash.Insert(10), "Should not allow duplicate insertion");
            Assert(hash.Count == 20, "Count should remain 20 after duplicate attempt");
            
            // Test finds
            Assert(hash.Find(15), "Should find element 15");
            Assert(!hash.Find(99), "Should not find element 99");
            
            // Test removals
            Assert(hash.Remove(15), "Should remove element 15");
            Assert(!hash.Find(15), "Should not find element 15 after removal");
            Assert(hash.Count == 19, "Count should be 19 after one removal");
            
            Console.WriteLine("  ✓ HashTableChaining tests passed");
        }

        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception($"Test failed: {message}");
            }
        }
    }
}