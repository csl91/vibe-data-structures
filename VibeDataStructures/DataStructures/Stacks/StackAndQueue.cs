using System;
using System.Collections.Generic;
using System.Diagnostics;
using VibeDataStructures.Interfaces;

namespace VibeDataStructures.DataStructures.Stacks
{
    /// <summary>
    /// Array-based Stack implementation
    /// 
    /// Time Complexity:
    /// - Insert (Push): O(1) amortized
    /// - Find: O(n) - linear search
    /// - Remove (Pop): O(1) for top element, O(n) for arbitrary element
    /// 
    /// Space Complexity: O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class ArrayStack<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 4;

        public int Count => _count;
        public bool IsEmpty => _count == 0;

        public ArrayStack(int initialCapacity = DefaultCapacity)
        {
            _items = new T[Math.Max(initialCapacity, DefaultCapacity)];
            _count = 0;
        }

        /// <summary>
        /// Pushes an element onto the stack
        /// O(1) amortized time complexity
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
        /// Finds an element in the stack using linear search
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
        /// O(1) for top element, O(n) for arbitrary element
        /// </summary>
        public bool Remove(T item)
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                if (_items[i].CompareTo(item) == 0)
                {
                    // Shift elements down
                    for (int j = i; j < _count - 1; j++)
                    {
                        _items[j] = _items[j + 1];
                    }
                    _count--;
                    _items[_count] = default(T)!;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Pops the top element from the stack
        /// O(1) time complexity
        /// </summary>
        public T Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Stack is empty");

            T item = _items[--_count];
            _items[_count] = default(T)!;
            return item;
        }

        /// <summary>
        /// Peeks at the top element without removing it
        /// O(1) time complexity
        /// </summary>
        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Stack is empty");
            return _items[_count - 1];
        }

        private void Resize()
        {
            T[] newItems = new T[_items.Length * 2];
            Array.Copy(_items, newItems, _count);
            _items = newItems;
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                _items[i] = default(T)!;
            }
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupStack = new ArrayStack<T>();
                foreach (var item in testData)
                    warmupStack.Insert(item);
            }

            // Benchmark Insert (Push)
            Clear();
            var sw = Stopwatch.StartNew();
            foreach (var item in testData)
            {
                Insert(item);
            }
            sw.Stop();
            
            results.Add(new PerformanceResult
            {
                DataStructureName = "ArrayStack",
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
                DataStructureName = "ArrayStack",
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
                DataStructureName = "ArrayStack",
                OperationName = "Remove",
                ElapsedTicks = sw.ElapsedTicks,
                ElapsedMilliseconds = sw.Elapsed.TotalMilliseconds,
                DataSize = testData.Length,
                Success = IsEmpty
            });

            return results;
        }
    }

    /// <summary>
    /// Circular array-based Queue implementation
    /// 
    /// Time Complexity:
    /// - Insert (Enqueue): O(1)
    /// - Find: O(n) - linear search
    /// - Remove (Dequeue): O(1) for front element, O(n) for arbitrary element
    /// 
    /// Space Complexity: O(n)
    /// </summary>
    /// <typeparam name="T">Type of elements stored</typeparam>
    public class CircularQueue<T> : IDataStructure<T>, IBenchmarkable<T> where T : IComparable<T>
    {
        private T[] _items;
        private int _front;
        private int _rear;
        private int _count;
        private int _capacity;

        public int Count => _count;
        public bool IsEmpty => _count == 0;
        public bool IsFull => _count == _capacity;

        public CircularQueue(int capacity = 16)
        {
            _capacity = Math.Max(capacity, 4);
            _items = new T[_capacity];
            _front = 0;
            _rear = -1;
            _count = 0;
        }

        /// <summary>
        /// Enqueues an element to the rear of the queue
        /// O(1) time complexity
        /// </summary>
        public bool Insert(T item)
        {
            if (IsFull)
            {
                Resize();
            }

            _rear = (_rear + 1) % _capacity;
            _items[_rear] = item;
            _count++;
            return true;
        }

        /// <summary>
        /// Finds an element in the queue using linear search
        /// O(n) time complexity
        /// </summary>
        public bool Find(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                int index = (_front + i) % _capacity;
                if (_items[index].CompareTo(item) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the first occurrence of an element
        /// O(1) for front element, O(n) for arbitrary element
        /// </summary>
        public bool Remove(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                int index = (_front + i) % _capacity;
                if (_items[index].CompareTo(item) == 0)
                {
                    // Shift elements to fill the gap
                    for (int j = i; j < _count - 1; j++)
                    {
                        int currentIndex = (_front + j) % _capacity;
                        int nextIndex = (_front + j + 1) % _capacity;
                        _items[currentIndex] = _items[nextIndex];
                    }
                    _count--;
                    _rear = (_rear - 1 + _capacity) % _capacity;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Dequeues an element from the front of the queue
        /// O(1) time complexity
        /// </summary>
        public T Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Queue is empty");

            T item = _items[_front];
            _items[_front] = default(T)!;
            _front = (_front + 1) % _capacity;
            _count--;
            return item;
        }

        /// <summary>
        /// Peeks at the front element without removing it
        /// O(1) time complexity
        /// </summary>
        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Queue is empty");
            return _items[_front];
        }

        private void Resize()
        {
            T[] newItems = new T[_capacity * 2];
            for (int i = 0; i < _count; i++)
            {
                newItems[i] = _items[(_front + i) % _capacity];
            }
            _items = newItems;
            _front = 0;
            _rear = _count - 1;
            _capacity *= 2;
        }

        public void Clear()
        {
            _front = 0;
            _rear = -1;
            _count = 0;
        }

        public List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3)
        {
            var results = new List<PerformanceResult>();
            
            // Warmup runs
            for (int w = 0; w < warmupRuns; w++)
            {
                var warmupQueue = new CircularQueue<T>();
                foreach (var item in testData)
                    warmupQueue.Insert(item);
            }

            // Benchmark Insert (Enqueue)
            Clear();
            var sw = Stopwatch.StartNew();
            foreach (var item in testData)
            {
                Insert(item);
            }
            sw.Stop();
            
            results.Add(new PerformanceResult
            {
                DataStructureName = "CircularQueue",
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
                DataStructureName = "CircularQueue",
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
                DataStructureName = "CircularQueue",
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