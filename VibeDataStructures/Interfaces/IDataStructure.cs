using System;

namespace VibeDataStructures.Interfaces
{
    /// <summary>
    /// Common interface for all data structures to ensure consistent benchmarking
    /// </summary>
    /// <typeparam name="T">Type of elements stored in the data structure</typeparam>
    public interface IDataStructure<T> where T : IComparable<T>
    {
        /// <summary>
        /// Inserts an element into the data structure
        /// </summary>
        /// <param name="item">Item to insert</param>
        /// <returns>True if insertion was successful, false otherwise</returns>
        bool Insert(T item);

        /// <summary>
        /// Finds an element in the data structure
        /// </summary>
        /// <param name="item">Item to find</param>
        /// <returns>True if item exists, false otherwise</returns>
        bool Find(T item);

        /// <summary>
        /// Removes an element from the data structure
        /// </summary>
        /// <param name="item">Item to remove</param>
        /// <returns>True if removal was successful, false if item not found</returns>
        bool Remove(T item);

        /// <summary>
        /// Gets the current number of elements in the data structure
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Checks if the data structure is empty
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Clears all elements from the data structure
        /// </summary>
        void Clear();
    }
}