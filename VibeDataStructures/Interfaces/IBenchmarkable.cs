using System;
using System.Collections.Generic;

namespace VibeDataStructures.Interfaces
{
    /// <summary>
    /// Performance metrics for benchmarking operations
    /// </summary>
    public class PerformanceResult
    {
        public string OperationName { get; set; } = string.Empty;
        public string DataStructureName { get; set; } = string.Empty;
        public long ElapsedTicks { get; set; }
        public double ElapsedMilliseconds { get; set; }
        public int DataSize { get; set; }
        public bool Success { get; set; }

        public override string ToString()
        {
            return $"{DataStructureName}.{OperationName}: {ElapsedMilliseconds:F4}ms (Size: {DataSize}, Success: {Success})";
        }
    }

    /// <summary>
    /// Interface for benchmarking data structures
    /// </summary>
    public interface IBenchmarkable<T> where T : IComparable<T>
    {
        /// <summary>
        /// Runs performance benchmarks for the data structure
        /// </summary>
        /// <param name="testData">Data to use for benchmarking</param>
        /// <param name="warmupRuns">Number of warmup runs before actual measurement</param>
        /// <returns>Performance results for each operation</returns>
        List<PerformanceResult> RunBenchmark(T[] testData, int warmupRuns = 3);
    }
}