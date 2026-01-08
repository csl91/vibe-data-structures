# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Project Overview

This repository implements various data structures to benchmark their performance for insert, find, and remove operations.

## Technology Stack

This project is written in .NET (C#).

## Architecture

This is a performance benchmarking project focused on data structure implementations. When code is added:
- Each data structure should be implemented as a separate module/class
- Benchmark code should be separated from data structure implementations
- Performance metrics (insert, find, remove) should be consistently measured across all implementations

## Development Guidelines

- When implementing new data structures, ensure they expose consistent interfaces for insert, find, and remove operations
- Include performance benchmarks for any new data structure implementations
- Use `Stopwatch` (from `System.Diagnostics`) to measure and output execution times for benchmarks
- Document time and space complexity for each operation in comments or documentation
