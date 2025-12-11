# Utility Project - Public Methods Reference

## Terminology Summary

**BFS (Breadth-First Search)** - Graph traversal algorithm that explores nodes level by level, guarantees shortest path
in unweighted graphs
**Dijkstra's Algorithm** - Finds shortest paths in weighted graphs by exploring nodes with lowest cost first

**A* (A-Star)** - Pathfinding algorithm that uses heuristics to guide search toward goal, more efficient than Dijkstra
for single target
**Flood Fill** - Algorithm that fills connected regions, like paint bucket tool in graphics programs

**GCD (Greatest Common Divisor)** - Largest positive integer that divides two numbers without remainder
**LCM (Least Common Multiple)** - Smallest positive integer that is divisible by two numbers

**Manhattan Distance** - Distance between two points calculated as sum of absolute differences of coordinates (|x1-x2| +
|y1-y2|)
**Euclidean Distance** - Straight-line distance between two points using Pythagorean theorem

**Modular Arithmetic** - Mathematical system where numbers "wrap around" after reaching a certain value (like clock
arithmetic)
**Permutations** - All possible arrangements of a collection (ABC can be arranged as ABC, ACB, BAC, BCA, CAB, CBA)

**Combinations** - Selections of items from collection where order doesn't matter (choosing 2 from ABC gives AB, AC, BC)
**Topological Sort** - Ordering of directed graph nodes where each node comes before its dependencies

**Prime Number** - Integer greater than 1 that has no positive divisors other than 1 and itself
**Sieve of Eratosthenes** - Efficient algorithm for finding all prime numbers up to a given limit

**Chinese Remainder Theorem** - Method for solving systems of simultaneous modular equations
**Extended Euclidean Algorithm** - Finds GCD and coefficients that satisfy Bézout's identity

## MainUtilities Class

**ToIntList(string, string)** - Converts a string to a list of integers with optional delimiter
**ToIntArray(string[])** - Converts string array to integer array using ToIntList

**ExtractPosInts(string)** - Extracts all positive integers from a string using regex
**ExtractInts(string)** - Extracts all integers (including negative) from a string using regex

**ExtractLongs(string)** - Extracts all long integers from a string using regex
**ExtractPosLongs(string)** - Extracts all positive long integers from a string using regex

**ExtractWords(string)** - Extracts all alphabetic words from a string using regex
**ToLongList(string, string)** - Converts string to list of longs with optional delimiter

**Repeat(string, int, string)** - Repeats text n times with optional separator
**Flatten<T>(T[,])** - Flattens 2D array into enumerable sequence

**JoinAsStrings<T>(IEnumerable<T>, char)** - Joins enumerable items as strings with char separator
**JoinAsStrings<T>(IEnumerable<T>, string)** - Joins enumerable items as strings with string separator

**SplitByNewline(string, bool, bool)** - Splits string by various newline formats with filtering options
**SplitByDoubleNewline(string, bool, bool)** - Splits string by double newlines for paragraph separation

**SplitIntoColumns(string)** - Transposes text input into column-based string array
**TrimArray(int[,], int, int)** - Removes specified row and column from 2D array

**FindGCD(double, double)** - Finds greatest common divisor for double values
**FindLCM(double, double)** - Finds least common multiple for double values

**FindGCD(long, long)** - Finds greatest common divisor for long values
**FindLCM(long, long)** - Finds least common multiple for long values

**ExtendedGCD(long, long)** - Extended Euclidean algorithm returning GCD and coefficients
**Mod(int, int)** - Positive modulo operation for integers

**Mod(long, long)** - Positive modulo operation for longs
**ModInverse(long, long)** - Calculates modular inverse using ModPower

**ModPower(long, long, long)** - Modular exponentiation using BigInteger
**Permutations<T>(IEnumerable<T>)** - Generates all permutations of collection elements

**Permutations<T>(IEnumerable<T>, int)** - Generates permutations of specific size from collection
**Combinations<T>(IEnumerable<T>, int)** - Generates all combinations of m elements from array

**Split<T>(IEnumerable<T>, int)** - Splits enumerable into chunks of specified size
**SplitAtIndex<T>(List<T>, int)** - Splits list at given index into two parts

**ToStringArray(char[][])** - Converts jagged char array to string array
**Rotate<T>(IEnumerable<T>, int)** - Rotates enumerable by specified number of positions

**Add((int,int), (int,int))** - Adds two 2D integer coordinate tuples
**Add((long,long), (long,long))** - Adds two 2D long coordinate tuples

**AllIndexesOf(string, string)** - Finds all starting indices of substring in string
**HexStringToBinary(string)** - Converts hexadecimal string to binary string representation

**Turn(CompassDirection, string, int)** - Turns compass direction left/right by degrees
**GetDirection<T>(Dictionary<(int,int), T>, (int,int), CompassDirection, T)** - Gets value in direction from dictionary

**KeyList<K,V>(Dictionary<K,V>, bool)** - Extracts keys from dictionary with optional sorting
**Neighbors(Coordinate2D, bool)** - Gets neighboring coordinates with optional diagonals

**GetImmediateNeighbors(Coordinate3D)** - Gets 6 immediate neighbors of 3D coordinate
**AStar(Coordinate2D, Coordinate2D, Dictionary<Coordinate2D, long>, out long, bool, bool)** - A* pathfinding algorithm

**GenerateMap(string, bool)** - Converts string input to coordinate-character dictionary map
**StringFromMap<TValue>(Dictionary<Coordinate2D, TValue>, int, int, bool)** - Converts coordinate map back to string

**Lines(string)** - Splits string into lines using StringReader
**Cells(string, Func<char, bool>)** - Converts string to Point2D-char dictionary with optional filtering

**AsMap<T>(string, Func<char, T>)** - Creates StringMap with custom selector function
**AsMap(string)** - Creates character-based StringMap from string input

**GetHighestSubsequence(string, int)** - Extracts lexicographically largest subsequence of target length
**GeneratePalindromicHalfNumbers(long, long)** - Generates numbers where left half equals right half

**HasRepeatedSequence(string)** - Checks if number string contains repeated patterns
**checkRepeats(string, int, int, string)** - Helper method for pattern repetition validation

## ExtendedDictionary<TKey, TValue> Class

**this[TKey]** - Indexer that auto-creates new TValue() if key doesn't exist
**this[TKey] set** - Sets value for key using base dictionary indexer

## Graph Class

**Graph(List<string>)** - Constructor that initializes graph from string input and calculates paths
**SavedP1** - Property containing list of saved values from wall calculations

**SavedP2** - Property containing list of saved values from path calculations
**BuildGraph(List<int>, List<(int,int)>)** - Static method to build directed graph from update rules

**TopologicalSort(Dictionary<int, List<int>>, List<int>)** - Static topological sort of graph nodes
**CalculateSavedWalls(List<int[]>, HashSet<(int,int,int)>)** - Calculates time savings by removing walls

**CalculateSavedPaths(HashSet<(int,int,int)>, int)** - Calculates time savings from path optimizations
**FindShortestPath(int[], int[])** - Finds shortest path between two positions in map

## Map Class

**Map(List<string>)** - Constructor to initialize map from list of string lines
**Lines** - Property containing original input lines

**Rows** - Property containing number of rows in map
**Columns** - Property containing number of columns in map

**GetDirectionValues(int[], string)** - Gets character values in specified directions from position
**GetDirectionPositions(int[], string)** - Gets positions in specified directions from given position

**GetColumnValues(int)** - Returns all character values in specified column
**GetRowValues(int)** - Returns all character values in specified row

**GetValueAtPosition(int[])** - Gets character value at specific row/column position
**OnBoard(int[])** - Checks if position coordinates are within map boundaries

**FindAll(char)** - Finds all positions containing specified target character
**PrintMap()** - Prints entire map to console for debugging

## Algorithms Class

**GCD(long, long)** - Calculates Greatest Common Divisor using Euclidean algorithm
**LCM(long, long)** - Calculates Least Common Multiple using GCD

**LCM(params long[])** - Calculates LCM of multiple numbers using aggregate
**LCM(IEnumerable<long>)** - Calculates LCM of enumerable collection of numbers

**GetPermutations<T>(IList<T>)** - Generates all possible permutations of list elements
**GetCombinations<T>(IList<T>, int)** - Generates combinations of k elements from list

**ManhattanDistance(int, int, int, int)** - Calculates Manhattan distance between two 2D points
**EuclideanDistance(double, double, double, double)** - Calculates Euclidean distance between two 2D points

**IsPrime(long)** - Checks if number is prime using trial division
**SieveOfEratosthenes(int)** - Generates all prime numbers up to limit using sieve

**ModPow(long, long, long)** - Calculates modular exponentiation efficiently
**ModInverse(long, long)** - Finds modular inverse using extended Euclidean algorithm

**ChineseRemainderTheorem(long[], long[])** - Solves system of linear congruences
**Factorial(int)** - Calculates factorial of non-negative integer

**BinomialCoefficient(int, int)** - Calculates binomial coefficient (n choose k)
**RotateLeft<T>(T[], int)** - Rotates array elements left by k positions

**RotateRight<T>(T[], int)** - Rotates array elements right by k positions
**GetDivisors(long)** - Finds all divisors of given number

## SetupInputFile Class

**OpenFile(string)** - Opens file and returns enumerable of lines
**GetSolutionDirectory()** - Finds solution directory by looking for .sln file

## TestFiles Class

**GetInputData(int, int, string)** - Constructs file path for AOC input data
**GetInputData** - Returns formatted path to specific day/year/file combination

## Grid Class

**Grid(string[])** - Constructor that creates grid from array of string lines
**Grid(char[,])** - Constructor that creates grid from 2D character array

**Rows** - Property returning number of rows in grid
**Cols** - Property returning number of columns in grid

**this[int, int]** - Indexer to get/set character at specific row/column
**this[Point2D<int>]** - Indexer to get/set character at Point2D coordinate

**IsInBounds(int, int)** - Checks if row/column coordinates are within grid bounds
**IsInBounds(Point2D<int>)** - Checks if Point2D coordinate is within grid bounds

**FindAll(char)** - Finds all Point2D positions containing specified character
**FindFirst(char)** - Finds first Point2D position containing specified character

**GetNeighbors(Point2D<int>)** - Gets valid 4-directional neighbors of position
**GetAllNeighbors(Point2D<int>)** - Gets valid 8-directional neighbors including diagonals

**Clone()** - Creates deep copy of grid instance
**ToStringArray()** - Converts grid back to string array format

**GetRawGrid()** - Returns copy of underlying 2D character array
**Count(char)** - Counts total occurrences of specific character in grid

**Print()** - Prints entire grid to console for debugging
**ToString()** - Converts grid to string representation with newlines

## Letters Class

**CountVowels(string)** - Counts number of vowel characters (aeiou) in string
**IsRepeatedLetter(string)** - Checks if string contains consecutive identical characters

**ContainsNoneOverlappingPair(string)** - Checks for non-overlapping character pairs in string
**ContainsNoneOverlappingPair** - Returns true if string has repeated pairs without overlap

## CompassDirection Enum

**N, NE, E, SE, S, SW, W, NW** - Eight compass directions with degree values
**Enumeration values** - Provides 8-way directional navigation with 45-degree increments

## Coordinate2D Class

**Coordinate2D(int, int)** - Constructor for 2D integer coordinate with x,y values
**Coordinate2D((int,int))** - Constructor accepting coordinate tuple as parameter

**RotateCW(int, Coordinate2D)** - Rotates coordinate clockwise around center point by degrees
**RotateCW(int)** - Rotates coordinate clockwise around origin by degrees

**RotateCCW(int, Coordinate2D)** - Rotates coordinate counter-clockwise around center point
**RotateCCW(int)** - Rotates coordinate counter-clockwise around origin

**operator +, -, *, ==, !=** - Arithmetic and comparison operators for coordinates
**ManDistance(Coordinate2D)** - Calculates Manhattan distance to another coordinate

**Equals(object)** - Checks equality with another coordinate object
**GetHashCode()** - Generates hash code for dictionary/set usage

**ToString()** - String representation in (x, y) format
**Deconstruct(out int, out int)** - Deconstructor for tuple assignment

## Coordinate2DL Class

**Coordinate2DL(long, long)** - Constructor for 2D long coordinate with x,y values
**Coordinate2DL((long,long))** - Constructor accepting long coordinate tuple

**RotateCW(int, Coordinate2DL)** - Rotates long coordinate clockwise around center
**RotateCCW(int, Coordinate2DL)** - Rotates long coordinate counter-clockwise around center

**operator +, -, *, ==, !=** - Arithmetic and comparison operators for long coordinates
**ManDistance(Coordinate2DL)** - Calculates Manhattan distance using long precision

**Equals(object)** - Checks equality with another long coordinate
**ToString()** - String representation in (x, y) format for long values

## Coordinate3D Class

**Coordinate3D(int, int, int)** - Constructor for 3D coordinate with x,y,z values
**Coordinate3D(string)** - Constructor parsing coordinate from string with integers

**Rotations** - Property returning list of all 24 possible 3D rotations
**operator +, -, ==, !=** - Arithmetic and comparison operators for 3D coordinates

**ManhattanDistance(Coordinate3D)** - Calculates 3D Manhattan distance to another coordinate
**ManhattanMagnitude()** - Calculates Manhattan distance from origin

**Equals(object)** - Checks equality with another 3D coordinate
**GetHashCode()** - Generates hash code using prime multiplication

**ToString()** - String representation in "x, y, z" format
**GetNeighbors()** - Static method returning array of 26 neighboring offsets

## Coordinate4D Class

**Coordinate4D(int, int, int, int)** - Constructor for 4D coordinate with x,y,z,w values
**operator +, -, ==, !=** - Arithmetic and comparison operators for 4D coordinates

**ManhattanDistance(Coordinate4D)** - Calculates 4D Manhattan distance to another coordinate
**Equals(object)** - Checks equality with another 4D coordinate

**GetHashCode()** - Generates hash code using prime multiplication for 4 dimensions
**GetNeighbors()** - Static method returning array of 80 neighboring 4D offsets

## Directions Class

**allDirections** - Static array containing all 8 directional movement vectors
**diagonals** - Static array containing 4 diagonal movement vectors only

## MoveDirections Class

**CompassDirectionFromArrow(char)** - Converts arrow character (^v<>) to CompassDirection
**Flip(CompassDirection)** - Returns opposite direction of given compass direction

**MoveDirection((int,int), CompassDirection, bool, int)** - Moves tuple coordinate in specified direction
**MoveDirection(Coordinate2D, CompassDirection, bool, int)** - Moves Coordinate2D in specified direction

**Get2dNeighborVals<T>(Dictionary<(int,int), T>, (int,int), T, bool)** - Gets neighbor values from tuple-keyed
dictionary
**Get2dNeighborVals<T>(Dictionary<Coordinate2D, T>, Coordinate2D, T, bool)** - Gets neighbor values from
Coordinate2D-keyed dictionary

## Parsing Class

**ExtractIntegers(string)** - Extracts all integers from text using regex
**ExtractLongs(string)** - Extracts all long integers from text using regex

**ExtractNumbers(string)** - Extracts all decimal numbers from text using regex
**SplitByEmptyLines(string[])** - Groups lines by empty line separators

**SplitByEmptyLines(IEnumerable<string>)** - Groups enumerable lines by empty separators
**BinaryToLong(string)** - Converts binary string to long integer

**HexToLong(string)** - Converts hexadecimal string to long integer
**CharToDigit(char, int)** - Converts character to digit value for specified base

**ParsePoint(string)** - Parses Point2D from "x,y" or "(x,y)" format
**CountFrequencies<T>(IEnumerable<T>)** - Creates frequency dictionary from collection

**Transpose(string[])** - Transposes 2D string array for grid rotation
**RotateClockwise(string[])** - Rotates string array 90 degrees clockwise

## PathFinding Class

**BFS(Point2D<int>, Func<Point2D<int>, bool>)** - Breadth-first search returning distance dictionary
**BFS(Grid, Point2D<int>, Func<char, bool>)** - BFS in grid context with cell validation

**FindPath(Point2D<int>, Point2D<int>, Func<Point2D<int>, bool>)** - Finds shortest path between two points using BFS
**Dijkstra(Point2D<int>, Func<Point2D<int>, Point2D<int>, int>, Func<Point2D<int>, bool>)** - Dijkstra's algorithm for
weighted graphs

**AStar(Point2D<int>, Point2D<int>, Func<Point2D<int>, bool>, ...)** - A* pathfinding with heuristic and cost functions
**FloodFill(Point2D<int>, Func<Point2D<int>, bool>)** - Flood fill algorithm returning connected cells

**FloodFill(Grid, Point2D<int>, char)** - Grid-based flood fill for matching characters
**FloodFill** - Returns HashSet of connected cells matching criteria

## Point2D<T> Class

**Point2D(T, T)** - Constructor for generic 2D point with X,Y coordinates
**CompareTo(Point2D<T>)** - Comparison method for sorting points

**ToString()** - String representation in "X,Y" format
**operator +, -, *, /, %, implicit conversions** - Arithmetic operators and tuple conversion

**Neighbours(bool)** - Gets 4 or 8 neighboring points based on diagonal parameter
**DiagonalNeighbours()** - Gets only the 4 diagonal neighboring points

**ManhattanDistance(Point2D<T>)** - Calculates Manhattan distance to another point
**Range(Point2D<T>, Point2D<T>)** - Generates all points in rectangular range

**Parse(string)** - Static method to parse Point2D from string
**DirectionFromArrow(char)** - Converts arrow character to directional Point2D

## DictMultiRange<T> Class

**DictMultiRange()** - Default constructor for keyed multi-range collection
**DictMultiRange(DictMultiRange<T>)** - Copy constructor for deep cloning

**Ranges** - Dictionary property mapping keys to Range objects
**len** - Property calculating total length by multiplying all range lengths

## MultiRange Class

**MultiRange()** - Default constructor for range collection
**MultiRange(IEnumerable<Range>)** - Constructor from enumerable of ranges

**MultiRange(MultiRange)** - Copy constructor for deep cloning ranges
**len** - Property calculating total length by multiplying range lengths

## Range Class

**Range(long, long)** - Constructor with start and end values
**Range(Range)** - Copy constructor for deep cloning range

**Start** - Property for range start value
**End** - Property for range end value

**Len** - Property calculating length (End - Start + 1)
**ToString()** - String representation showing start, end and length

## StringMap<T> Class

**StringMap(string, Func<char, T>)** - Constructor creating map from string with character selector
**Width** - Property returning map width

**Height** - Property returning map height
**Size** - Property returning Point2D size

**this[Point2D<int>]** - Indexer to get/set values at Point2D coordinates
**Contains(Point2D<int>)** - Checks if coordinates are within map bounds

**GetValueOrDefault(Point2D<int>)** - Gets value at coordinates or default if out of bounds
**TryGetValue(Point2D<int>, out T)** - Attempts to get value with success indicator

**Rows()** - Returns enumerable of rows as value sequences
**GetEnumerator()** - Iterator returning (Index, Value) tuples for enumeration

## LinearSystemSolver Class

Solves systems of linear equations with integer coefficients, used for button-effect puzzles (e.g., Advent of Code Day
10).

**Constructor:**

- `LinearSystemSolver(List<int> goal, List<List<int>> buttonEffects)`
    - `goal`: Target values for each equation
    - `buttonEffects`: List of which equations each button affects

**Public Methods:**

- `int Solve()` — Returns the minimum number of button presses to reach the goal, or 0 if unsolvable.

**Private Methods:**

- `BuildCoefficientMatrix(List<List<int>> buttonEffects)` — Builds the coefficient matrix for the system.
- `PerformGaussianElimination()` — Reduces the matrix to row-echelon form using integer arithmetic.
- `FindPivotRow(int startRow, int column)` — Finds the next pivot row in a column.
- `EliminateColumn(MatrixRowOperations rowOps, int pivotRow, int pivotColumn)` — Eliminates a column using the pivot
  row.
- `ShouldSkipRow(int rowIndex, int pivotRow, int pivotColumn)` — Checks if a row should be skipped during elimination.
- `IdentifyFreeVariables()` — Identifies variables not bound by pivots (free variables).
- `CalculateSearchBound()` — Determines the maximum value to try for free variables.
- `Search(int freeVarIndex)` — Recursively searches for valid assignments to free variables.
- `ShouldStopSearch()` — Checks if the search should stop due to iteration limit.
- `IsSearchComplete(int freeVarIndex)` — Checks if all free variables have been assigned.
- `TryUpdateBestSolution()` — Updates the best solution if a valid assignment is found.
- `SearchFreeVariableValues(int freeVarIndex)` — Iterates through possible values for a free variable.
- `TrySolvePivotVariables()` — Attempts to solve for pivot variables given current free variable assignments.
- `TrySolvePivotVariable(int rowIndex, out long value)` — Solves a single pivot variable.
- `CalculateAdjustedRightHandSide(int rowIndex, int pivotVarIndex)` — Calculates the right-hand side for a pivot
  variable.
- `VerifySolution()` — Checks if the current solution satisfies all equations.
- `CalculateEquationSum(int equationIndex)` — Calculates the sum for a single equation.

## MatrixRowOperations Class

Encapsulates row operations for Gaussian elimination on integer matrices.

**Constructor:**

- `MatrixRowOperations(long[,] matrix, long[] rightHandSide, int columnCount)`

**Methods:**

- `SwapRows(int row1, int row2)` — Swaps two rows in the matrix and right-hand side.
- `EliminateRow(int targetRow, int pivotRow, int pivotColumn)` — Performs row elimination using the pivot row.
- `ReduceRowByGcd(int rowIndex)` — Reduces all coefficients in a row by their greatest common divisor.
- `CalculateRowGcd(int rowIndex)` — Calculates the GCD of all coefficients in a row (private).

---

**Note:**

- All method names are chosen to clearly reflect their intent and purpose.
- Matrix row operations are consolidated in a dedicated class to avoid duplication and improve clarity.
- The solver is designed for integer systems and uses brute-force search for free variables, with a configurable
  iteration limit.
