using Utility;

namespace AOC2024;

public class Day24
{
  private static readonly List<string> instructions = new();
  private static readonly Dictionary<string, int> registers = new();

  private static readonly HashSet<(string x, string y, string type, string output)> gates = [];
  private static readonly HashSet<string> PossibleSwaps = [];

  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    bool isLoadRegister = true;
    foreach (string? line in data)
    {
      if (line.Trim() == "")
      {
        isLoadRegister = false;
        continue;
      }

      if (isLoadRegister)
      {
        string[]? reg = line.Split(':', StringSplitOptions.TrimEntries);
        registers[reg[0]] = int.Parse(reg[1]);
        continue;
      }

      string[]? split = line.Split("->");
      string[]? terms = split[0].Split(" ");
      gates.Add((terms[0], terms[2], terms[1], split[1].Trim()));

      instructions.Add(line);
    }

    string? result1 = ProcessLogic();
    string? result2 = CheckPossibleSwaps();
    return (result1, result2);
  }
  private static string ProcessLogic()
  {
    var processed = new HashSet<string>();

    while (processed.Count < instructions.Count)
    {
      foreach (string? instruction in instructions)
      {
        if (processed.Contains(instruction)) continue;

        string[]? parts = instruction.Split(new[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2) continue;

        string? expression = parts[0].Trim();
        string? result = parts[1].Trim();

        // Parse and evaluate the expression
        if (EvaluateExpression(expression, result))
        {

          processed.Add(instruction);
        }
      }
    }


    // Filter and sort registers with keys starting with "z"
    var zRegisters = registers.Where(kv => kv.Key.StartsWith("z")).OrderBy(kv => kv.Key).Select(kv => kv.Value).Reverse()
      .ToList();

    // Construct the binary string
    string binaryString = string.Join("", zRegisters);

    // Convert binary string to decimal
    long decimalValue = Convert.ToInt64(binaryString, 2);

    return $"{decimalValue}";
  }

  private static bool EvaluateExpression(string expression, string result)
  {
    int value = 0;
    string[]? tokens = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

    string op1 = tokens[0], op = tokens[1], op2 = tokens[2];
    if (!registers.TryGetValue(op1, out int val1) && !int.TryParse(op1, out val1)) return false;
    if (!registers.TryGetValue(op2, out int val2) && !int.TryParse(op2, out val2)) return false;

    value = op switch
    {
      "AND" => val1 & val2,
      "OR" => val1 | val2,
      "XOR" => val1 ^ val2,
      _ => throw new InvalidOperationException($"Unsupported operation: {op}")
    };
    if (registers.ContainsKey(result))
    {
      registers[result] = value;
      return true;
    }

    registers.Add(result, value);
    return true;
  }

  private static string CheckPossibleSwaps()
  {
    int zcount = registers.Count(kv => kv.Key.StartsWith('z'));

    //46
    string lastZ = $"z{zcount - 1:00}";
    string firstcheck = string.Empty;
    for (int x = 0; x < zcount - 1; x++)
    {
      string num = $"{x:D2}";
      string firstgate = string.Empty;
      string secondgate = string.Empty;
      string andgate = string.Empty;
      string xorgate = string.Empty;
      string orgate = string.Empty;

      firstgate = FindOutput($"x{num}", $"y{num}", "XOR");
      secondgate = FindOutput($"x{num}", $"y{num}", "AND");


      if (!string.IsNullOrWhiteSpace(firstcheck))
      {
        andgate = FindOutput(firstcheck, firstgate, "AND");

        if (string.IsNullOrWhiteSpace(andgate))
        {
          (secondgate, firstgate) = (firstgate, secondgate);
          PossibleSwaps.Add(firstgate);
          PossibleSwaps.Add(secondgate);
          andgate = FindOutput(firstcheck, firstgate, "AND");
        }

        xorgate = FindOutput(firstcheck, firstgate, "XOR");

        if (firstgate.StartsWith('z'))
        {
          (firstgate, xorgate) = (xorgate, firstgate);
          PossibleSwaps.Add(firstgate);
          PossibleSwaps.Add(xorgate);
        }


        if (secondgate.StartsWith('z'))
        {
          (secondgate, xorgate) = (xorgate, secondgate);
          PossibleSwaps.Add(secondgate);
          PossibleSwaps.Add(xorgate);
        }


        if (andgate.StartsWith('z'))
        {
          (andgate, xorgate) = (xorgate, andgate);
          PossibleSwaps.Add(andgate);
          PossibleSwaps.Add(xorgate);
        }


        orgate = FindOutput(andgate, secondgate, "OR");
      }

      if (orgate.StartsWith('z') && orgate != lastZ)
      {
        (orgate, xorgate) = (xorgate, orgate);
        PossibleSwaps.Add(orgate);
        PossibleSwaps.Add(xorgate);
      }


      firstcheck = string.IsNullOrWhiteSpace(firstcheck) ?
        secondgate :
        orgate;
    }

    return string.Join(",", PossibleSwaps.Order());
  }
  private static string FindOutput(string a, string b, string operatorType)
  {
    return gates.FirstOrDefault(gate
        => gate.x == a && gate.y == b && gate.type == operatorType || gate.x == b && gate.y == a && gate.type == operatorType)
      .output;
  }
}