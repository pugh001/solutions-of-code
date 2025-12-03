using System.Text.RegularExpressions;
using Utility;

namespace AOC2024;

public class Day13
{
  public (string, string) Process(string input)
  {
    long result1 = 0, result2 = 0;
    var data = SetupInputFile.OpenFile(input);
    var regex = new Regex(@"X[\+=](\d+), Y[\+=](\d+)");
    long buttonAx = 0, buttonAy = 0, buttonBx = 0, buttonBy = 0, answerAx = 0, answerAy = 0;
    long counter = 0;

    foreach (string line in data)
    {
      if (counter == 3)
      {
        result1 += FindCombination(answerAx, answerAy, buttonAx, buttonAy, buttonBx, buttonBy, 100);
        answerAx += 10000000000000;
        answerAy += 10000000000000;
        result2 += VerySimplePart2(answerAx, answerAy, buttonAx, buttonAy, buttonBx, buttonBy);
        buttonAx = buttonAy = buttonBx = buttonBy = answerAx = answerAy = 0;
        counter = 0;
        continue;
      }

      var match = regex.Match(line);
      if (match.Success)
      {
        long xValue = long.Parse(match.Groups[1].Value);
        long yValue = long.Parse(match.Groups[2].Value);

        switch (counter)
        {
          case 0:
            buttonAx = xValue;
            buttonAy = yValue;
            break;
          case 1:
            buttonBx = xValue;
            buttonBy = yValue;
            break;
          case 2:
            answerAx = xValue;
            answerAy = yValue;
            break;
        }
      }

      counter++;
    }

    if (answerAx > 0 || buttonAx > 0 || buttonBx > 0)
      Console.WriteLine("Error");

    return (result1.ToString(), result2.ToString());
  }


  private static long FindCombination(long prizeX, long prizeY, long ax, long ay, long bx, long by, long loopLimit)
  {
    for (long countA = 0; countA <= loopLimit; countA++)
    {
      for (long countB = 0; countB <= loopLimit; countB++)
      {
        long currentX = countA * ax + countB * bx;
        long currentY = countA * ay + countB * by;
        if (currentX == prizeX && currentY == prizeY)
          return countA * 3 + countB;
      }
    }

    return 0;
  }

  private static long VerySimplePart2(long answerAx, long answerAy, long buttonAx, long buttonAy, long buttonBx, long buttonBy)
  {
    // Calculate the denominator for solving B
    long denominator = buttonBy * buttonAx - buttonBx * buttonAy;

    // Calculate the numerator for B
    long numeratorB = answerAy * buttonAx - answerAx * buttonAy;

    // Solve for B
    long sovleforB = numeratorB / denominator;

    // Calculate the remaining X distance after accounting for B presses
    long remainingX = answerAx - sovleforB * buttonBx;

    // Solve for A
    long solveForA = remainingX / buttonAx;

    // Verify that solveForA and solveForB are non-negative
    bool areNonNegative = solveForA >= 0 && sovleforB >= 0;

    // Verify that the X-coordinate aligns
    bool isXAligned = sovleforB * buttonBx + solveForA * buttonAx == answerAx;

    // Verify that the Y-coordinate aligns
    bool isYAligned = sovleforB * buttonBy + solveForA * buttonAy == answerAy;

    // Add to result if all conditions are satisfied
    if (areNonNegative && isXAligned && isYAligned)
    {
      return solveForA * 3 + sovleforB;
    }

    return 0;
  }
}