# Solutions of Code - C# Implementation Collection

This repository contains C# solutions for competitive programming challenges including Advent of Code and LeetCode problems. The project is structured as a comprehensive solution with shared utilities and automated testing frameworks.

## 📁 Project Overview

### 🎄 Advent of Code Solutions

#### AOC2015 - Advent of Code 2015
- **Status**: Completed (8 days) 
- **Features**: Automated testing framework with xUnit integration
- **Structure**: 25 daily challenges (day1-day25), each with Part 1 and Part 2 solutions
- **Testing**: TestData.txt containing expected answers for validation
- **Input Management**: Separate example files (Part1Example.txt, Part2Example.txt) and puzzle input

#### AOC2024 - Advent of Code 2024  
- **Status**: Active development with dynamic program execution
- **Features**: Enhanced with DynamicProgram.cs for runtime optimization
- **Structure**: Similar 25-day structure with improved performance monitoring
- **Testing**: Comprehensive test coverage with timing measurements
- **Special**: Includes both standard Program.cs and DynamicProgram.cs for different execution modes

#### AOC2025 - Advent of Code 2025 (Current)
- **Status**: In progress (current year)
- **Features**: Updated framework with improved file structure
- **Documentation**: Enhanced README.md with setup instructions
- **Testing**: Streamlined test data management
- **Workflow**: 4-step process for each day (puzzle input → test data → example data → implementation)

### 🧩 LeetCode Solutions

#### LeetCode Project
- **Problems Solved**: 20+ problems across various difficulty levels
- **Problem Range**: #3 (Longest Substring) to #3427 (recent problems)
- **Categories**: Arrays, strings, graphs, dynamic programming, and more
- **Structure**: Each problem in separate folder with Solution.cs and problem description
- **Examples**: 
  - Problem #3: Longest Substring Without Repeating Characters
  - Problem #53: Maximum Subarray
  - Problem #322: Coin Change
  - Problem #973: K Closest Points to Origin

### 🛠️ Utility Library

#### Comprehensive Utility Project
A robust shared library providing essential algorithms and data structures:

**Core Utilities:**
- **String Processing**: Parsing, extraction, splitting, and manipulation
- **Mathematical Operations**: GCD, LCM, modular arithmetic, permutations, combinations
- **Pathfinding Algorithms**: BFS, Dijkstra, A*, flood fill
- **Geometric Calculations**: Manhattan distance, Euclidean distance, coordinate transformations
- **Grid Operations**: 2D/3D grid manipulation, rotation, neighbor detection

**Key Classes:**
- `Map`: Grid-based operations with direction handling
- `Grid`: Enhanced 2D grid with bounds checking and neighbor detection
- `Graph`: Graph algorithms including topological sort
- `Coordinate2D/3D/4D`: Multi-dimensional coordinate handling
- `PathFinding`: Comprehensive pathfinding algorithms
- `StringMap<T>`: Generic string-based mapping utilities

**Advanced Features:**
- Prime number generation (Sieve of Eratosthenes)
- Chinese Remainder Theorem implementation
- Extended Euclidean Algorithm
- Rotational transformations and compass directions

### ⚡ SpeedTestingApp
- **Purpose**: Performance benchmarking and algorithm optimization testing
- **Status**: Framework ready for implementation
- **Use Case**: Comparing algorithm performance across different implementations

## 🔧 Technical Architecture

### Solution Structure
- **Framework**: .NET with C# 
- **Testing**: xUnit framework with automated test discovery
- **Organization**: Modular design with shared utilities
- **Build System**: MSBuild with project references

### Development Workflow

**For Advent of Code:**
1. Add puzzle input to `puzzleInput.txt`
2. Update test data in `Answers/TestData.txt`
3. Add example data to `part1Example.txt`
4. Implement solution in `Day#.cs`

**For LeetCode:**
- Create problem folder with Solution.cs and problem.md
- Implement solution following LeetCode format requirements

### Testing Framework
- Automated test execution for all implemented days
- Expected vs actual result validation
- Performance timing measurements
- Continuous integration ready

## 🎯 Key Features

- **Automated Testing**: Complete test coverage for all solutions
- **Performance Monitoring**: Execution time tracking for optimization
- **Reusable Components**: Comprehensive utility library for common algorithms
- **Clean Architecture**: Modular design with clear separation of concerns
- **Documentation**: Extensive inline documentation and utility reference guide

## 🚀 Getting Started

1. Clone the repository
2. Open `SolutionOfCode.sln` in Visual Studio or your preferred IDE
3. Build the solution to restore dependencies
4. Run tests to verify setup: `dotnet test`
5. Execute specific year: `dotnet run --project AOC2024`

## 📊 Current Status

- **AOC 2015**: 8 days complete
- **AOC 2024**: ✅ Complete  
- **AOC 2025**: 🚧 In Progress
- **LeetCode**: 📈 20+ Problems Solved
- **Utility Library**: 📚 Comprehensive Algorithm Collection
