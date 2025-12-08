using Utility;

namespace AOC2025;

//Stage 2 : Final Zero Crossings: 5978
public class Day1
{
  private int _current = 50;
  private int _zeroCrossings;
  private int _zeros;
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    InitializeDial();

    foreach (string line in data)
    {
      if (TryParseInstruction(line, out char direction, out int value))
      {
        MoveDial(direction, value);
        if (_current == 0) _zeros++;
      }

    }

    return (_zeros.ToString(), _zeroCrossings.ToString());
  }

  private void InitializeDial()
  {
    _current = 50;
    _zeros = 0;
    _zeroCrossings = 0;
  }

  private bool TryParseInstruction(string instruction, out char direction, out int value)
  {
    direction = '?';
    value = 0;

    if (string.IsNullOrEmpty(instruction) || instruction.Length < 2)
      return false;

    direction = instruction[0];
    if (direction != 'L' && direction != 'R')
      return false;

    var integers = instruction.ExtractPosInts().ToList();
    if (integers.Count != 1)
      return false;

    value = integers[0];
    return true;
  }

  private void MoveDial(char direction, int value)
  {
    switch (direction)
    {
      case 'L':
        MoveLeft(value);
        break;
      case 'R':
        MoveRight(value);
        break;
    }
  }

  private void MoveLeft(int steps)
  {
    // Calculate new position
    int newPosition = _current - steps;

    // Count how many times we cross through 0 (going from 1 to 0 or wrapping from negative to 99)
    if (_current > 0 && newPosition <= 0)
    {
      _zeroCrossings++; // Cross zero going backwards from positive to 0
    }

    // Count additional crossings from wrapping around negative values
    if (newPosition < 0)
    {
      _zeroCrossings += Math.Abs(newPosition) / 100;
    }

    // Update position using proper modular arithmetic
    _current = MathUtilities.Mod(newPosition, 100);
  }

  private void MoveRight(int steps)
  {
    // Calculate new position
    int newPosition = _current + steps;

    // Count how many times we cross through 0 (reaching 100 wraps to 0)
    _zeroCrossings += newPosition / 100;

    // Update position using proper modular arithmetic
    _current = MathUtilities.Mod(newPosition, 100);
  }
}