using Utility;

namespace AOC2024;

public class Day7
{
  private static char[] _operators = null!;

  public (string, string) Process(string input)
  {
    // Load and parse input data
    string[] data = SetupInputFile.OpenFile(input).ToArray();
    return (ProcessPart1(data).ToString(), ProcessPart2(data).ToString());
  }

  private static long ProcessPart1(string[] data)
  {
    _operators = ['*', '+'];
    return Solve(data);

  }

  private static long ProcessPart2(string[] data)
  {
    _operators = ['*', '+', '|'];
    return Solve(data);

  }


  private static long Concatenate(string left, string right)
  {
    string concatenated = left + right;
    return long.Parse(concatenated);
  }

  // Method to evaluate the expression from left to right
  private static long EvaluateExpression(List<long> numbers, List<char> operators)
  {
    long result = numbers[0];
    for (int i = 1; i < numbers.Count; i++)
    {
      switch (operators[i - 1])
      {
        case '+':
          result += numbers[i];
          break;
        case '*':
          result *= numbers[i];
          break;
        case '|':
          result = Concatenate(result.ToString(), numbers[i].ToString());
          break;
      }
    }

    return result;
  }

  // Method to generate all possible operator combinations
  private static bool CheckValidEquation(long testValue, List<long> numbers)
  {
    long numCount = numbers.Count;
    if (numCount == 1) return false; // No operators needed if only one number

    var operatorCombinations = GenerateOperatorCombinations(numCount - 1);

    foreach (var ops in operatorCombinations)
    {
      if (EvaluateExpression(numbers, ops) == testValue)
        return true;
    }

    return false;
  }

  // Method to generate all possible combinations of '+' and '*' operators
  private static List<List<char>> GenerateOperatorCombinations(long length)
  {
    var result = new List<List<char>>();


    // Generate all combinations of '+' and '*' with length 'length'
    GenerateOperatorsRecursively(result, new List<char>(), length, _operators);

    return result;
  }

  // Helper method to generate combinations recursively
  private static void GenerateOperatorsRecursively(List<List<char>> result, List<char> current, long remaining, char[] operators)
  {
    if (remaining == 0)
    {
      result.Add(new List<char>(current));
      return;
    }

    foreach (char op in operators)
    {
      current.Add(op);
      GenerateOperatorsRecursively(result, current, remaining - 1, operators);
      current.RemoveAt(current.Count - 1); // Backtrack
    }
  }

  // Method to solve the entire problem
  private static long Solve(string[] inputLines)
  {
    long totalCalibrationResult = 0;

    foreach (string line in inputLines)
    {
      // Parse the line longo the test value and numbers
      string[] parts = line.Split(":");
      long testValue = long.Parse(parts[0].Trim());
      string[] numbersStr = parts[1].Trim().Split(" ");
      var numbers = new List<long>();

      foreach (string num in numbersStr)
      {
        numbers.Add(long.Parse(num));
      }

      // Check if the equation is valid
      if (CheckValidEquation(testValue, numbers))
      {
        totalCalibrationResult += testValue;
      }
    }

    return totalCalibrationResult;
  }
}