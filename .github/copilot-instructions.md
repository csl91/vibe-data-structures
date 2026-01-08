# AI Coding Instructions for vibe-data-structures

## Project Overview
This is a .NET (C#) benchmarking project focused on implementing and comparing the performance of various data structures for insert, find, and remove operations.

## Architecture & Structure

### Core Design Patterns
- **Consistent Interface**: All data structures must implement consistent interfaces for `Insert()`, `Find()`, and `Remove()` operations
- **Separation of Concerns**: Keep data structure implementations separate from benchmark/testing code
- **Performance-First**: Every implementation should be optimized and include time/space complexity documentation

### Expected Project Structure
```
/DataStructures/          # Individual data structure implementations
  /Arrays/
  /LinkedLists/
  /Trees/
  /HashTables/
/Benchmarks/              # Performance testing and comparison code
/Tests/                   # Unit tests for correctness
```

## Development Guidelines

### Performance Benchmarking
- Use `System.Diagnostics.Stopwatch` for precise timing measurements
- Benchmark all three core operations: Insert, Find, Remove
- Include warm-up runs to account for JIT compilation
- Test with various data sizes (small, medium, large datasets)
- Output results in consistent format for comparison

### Data Structure Implementation
- Document Big O complexity for each operation in XML comments
- Include both best-case and worst-case scenarios
- Implement proper error handling for edge cases (empty structure, not found, etc.)
- Consider generic implementations (`<T>`) where appropriate

### Code Conventions
- Follow standard C# naming conventions (PascalCase for methods, camelCase for variables)
- Use meaningful variable names that reflect the data structure concepts
- Add comprehensive XML documentation for public APIs
- Include example usage in documentation

## Key Files to Reference
- [WARP.md](../WARP.md) - Contains additional development guidelines
- [README.md](../README.md) - Project overview and purpose

## Testing Strategy
- Unit tests for correctness before performance optimization
- Edge case testing (empty, single element, large datasets)
- Comparative benchmarks between similar data structures
- Memory usage profiling alongside execution time

## Common Pitfalls to Avoid
- Don't optimize prematurely - correctness first, then performance
- Avoid platform-specific optimizations unless clearly documented
- Don't forget to test with different data types and comparison functions
- Ensure fair comparison by using identical test datasets across implementations