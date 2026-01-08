# Vibe Data Structures - Performance Benchmarking Project

A comprehensive .NET (C#) project for implementing and benchmarking various data structures to compare their performance for Insert, Find, and Remove operations.

## 🏗️ Architecture

This project implements **10 fundamental data structures** with consistent interfaces and comprehensive benchmarking:

### Data Structures Included

1. **Dynamic Array** - Resizable array (like `List<T>`)
2. **Static Array** - Fixed-capacity array
3. **Singly Linked List** - Single-direction linked nodes
4. **Doubly Linked List** - Bidirectional linked nodes
5. **Binary Search Tree** - Unbalanced BST
6. **Hash Table (Chaining)** - Collision resolution via linked lists
7. **Hash Table (Open Addressing)** - Linear probing collision resolution
8. **Binary Min Heap** - Array-based heap structure
9. **Array Stack** - LIFO stack using resizable array
10. **Circular Queue** - FIFO queue using circular array

### Project Structure

```
VibeDataStructures/
├── Interfaces/                 # Common interfaces (IDataStructure, IBenchmarkable)
├── DataStructures/
│   ├── Arrays/                # DynamicArray, StaticArray
│   ├── LinkedLists/           # SinglyLinkedList, DoublyLinkedList
│   ├── Trees/                 # BinarySearchTree
│   ├── HashTables/            # HashTableChaining, HashTableOpenAddressing
│   ├── Heaps/                 # BinaryMinHeap
│   └── Stacks/                # ArrayStack, CircularQueue
├── Benchmarks/                # Performance testing framework
├── Tests/                     # Basic correctness tests
└── Program.cs                 # Main entry point and CLI
```

## 🚀 Quick Start

### Build and Run

```bash
# Build the project
dotnet build

# Run basic correctness tests
dotnet run test

# Quick demo with small data sets
dotnet run quick

# List all available data structures
dotnet run list

# Run comprehensive benchmark (default)
dotnet run

# Compare specific data structures
dotnet run compare DynamicArray HashTableChaining
dotnet run compare BinarySearchTree HashTableOpenAddressing
```

### Sample Output

The quick demo shows basic functionality:

```
=== Dynamic Array Demo ===
Inserted 5 elements. Count: 5
Find 30: True
Remove 30: True
Count after removal: 4

=== Mini Benchmark (1000 elements) ===
Testing HashTableChaining:
  Insert: 0.0515ms (Success: True)
  Find: 0.0092ms (Success: True) 
  Remove: 0.0117ms (Success: True)
```

## 📊 Performance Characteristics

Each data structure is documented with Big O complexity:

| Data Structure | Insert | Find | Remove | Notes |
|----------------|--------|------|---------|-------|
| Dynamic Array | O(1)* | O(n) | O(n) | *Amortized, O(n) when resizing |
| Hash Table (Chaining) | O(1)* | O(1)* | O(1)* | *Average case, O(n) worst case |
| Binary Search Tree | O(log n)* | O(log n)* | O(log n)* | *Average case, O(n) if unbalanced |
| Binary Min Heap | O(log n) | O(n) | O(log n) | O(1) for peek minimum |
| Singly Linked List | O(1) | O(n) | O(n) | Insert at head |

## 🔧 Key Features

### Consistent Interface Design
All data structures implement `IDataStructure<T>` with standardized operations:
- `bool Insert(T item)` - Add element
- `bool Find(T item)` - Search for element  
- `bool Remove(T item)` - Delete element
- `int Count` - Number of elements
- `void Clear()` - Empty the structure

### Comprehensive Benchmarking
- **Warmup runs** to account for JIT compilation
- **Multiple test sizes** (100, 1000, 5000, 10000 elements)
- **Sequential and shuffled data** testing
- **Performance ranking** by operation type
- **Accurate timing** using `System.Diagnostics.Stopwatch`

### Developer-Friendly Testing
- **Basic correctness tests** verify functionality
- **Focused comparisons** between specific structures
- **Command-line interface** for easy usage
- **Detailed documentation** with complexity analysis

## 🎯 Usage Examples

### Compare Hash Table Implementations
```bash
dotnet run compare HashTableChaining HashTableOpenAddressing
```

### Test with Different Data Patterns
The benchmark automatically tests both:
- **Sequential data** (1, 2, 3, ...) - best case for some structures
- **Shuffled data** - reveals worst-case performance

### Extend with New Data Structures
1. Implement `IDataStructure<T>` and `IBenchmarkable<T>`
2. Add factory method to `BenchmarkRunner`
3. Include in comprehensive benchmarks

## 📈 Performance Insights

From initial testing with 1000 elements:
- **Hash Tables** excel at all operations (~0.01-0.05ms)
- **Dynamic Arrays** fast insert (0.006ms), slower find/remove (~0.6-1.0ms)
- **Binary Search Trees** consistent but slower with sequential data (~5ms)
- **Heaps** optimize for min/max operations

## 🔍 Development Guidelines

### Code Standards
- **XML documentation** for all public APIs
- **Big O complexity** documented in comments
- **Consistent naming** following C# conventions
- **Error handling** for edge cases (empty structures, etc.)

### Performance Focus
- **JIT warmup** before measurements
- **Memory-conscious** implementations
- **Cache-friendly** data layouts where possible
- **Realistic test scenarios** with various data sizes

## 🛠️ Future Enhancements

Potential additions for expanded benchmarking:
- Self-balancing trees (AVL, Red-Black)
- Advanced hash tables (Robin Hood, Cuckoo)
- Concurrent data structures
- Memory usage profiling
- Comparison with .NET built-in collections

---

This project provides a solid foundation for understanding data structure performance characteristics and serves as a practical learning tool for algorithm analysis and optimization techniques.
