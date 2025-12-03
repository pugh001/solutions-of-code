using Utility;

namespace AOC2015;

public class Day7
{
  private static readonly Dictionary<string, string> instructions = new();
  private static readonly Dictionary<string, ushort> cache = new();

  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input).ToList();
    //Part 2 just adjust the input data and run again.
    return (ProcessLogic(data), "");
  }
  private static string ProcessLogic(List<string> data)
  {

    foreach (string[]? parts in data.Select(action => action.Split(" -> ")))
    {
      instructions[parts[1]] = parts[0];
    }

    ushort x = GetSignal("a");
    return x.ToString();
  }
  private static ushort GetSignal(string wire)
  {
    if (cache.TryGetValue(wire, out ushort signal))
      return signal;

    if (ushort.TryParse(wire, out ushort value)) // If it's a direct number
      return value;

    if (!instructions.ContainsKey(wire))
      throw new Exception($"No instruction for wire: {wire}");

    string instruction = instructions[wire];
    ushort result = ProcessInstruction(instruction);
    cache[wire] = result; // Cache the result for future use
    return result;
  }
  private static ushort ProcessInstruction(string instruction)
  {
    string[] parts = instruction.Split(' ');

    switch (parts.Length)
    {
      // Direct assignment
      case 1:
        return GetSignal(parts[0]);
      // NOT operation
      case 2:
      {
        ushort operand = GetSignal(parts[1]);
        return (ushort)~operand;
      }
      // Binary operations
      default:
      {
        ushort operand1 = GetSignal(parts[0]);
        ushort operand2 = GetSignal(parts[2]);
        string operation = parts[1];

        return operation switch
        {
          "AND" => (ushort)(operand1 & operand2),
          "OR" => (ushort)(operand1 | operand2),
          "LSHIFT" => (ushort)(operand1 << int.Parse(parts[2])),
          "RSHIFT" => (ushort)(operand1 >> int.Parse(parts[2])),
          _ => throw new Exception($"Unknown operation: {operation}")
        };
      }
    }
  }
}