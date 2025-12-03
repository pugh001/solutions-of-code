using System.Text;

namespace AOC2024;

public class Day17
{
  private static long _registerA = 21539243;
  private static long _registerB;
  private static long _registerC;
  private static List<int> _commandList = [];
  private static long _result2;
  public (string, string) Process(string input)
  {
    if (input.Contains("Example"))
    {
      _commandList = [];
      _registerA = 21539243;
      _registerB = 0;
      _registerC = 0;
    }

    long result1 = -1;

    const string progStr = "2,4,1,3,7,5,1,5,0,3,4,1,5,5,3,0";
    string[] programIn = progStr.Split(',');
    _commandList = [..programIn.Select(op => int.Parse(op))];

    string programOut = RunProcess(_commandList, _registerA, _registerB, _registerC).TrimEnd(',');

    CalculateRegisterA(_commandList.Count - 1, 0);

    return (programOut, _result2.ToString());
  }
  private static bool CalculateRegisterA(int lengthOfOperandList, long registerAtrying)
  {
    if (lengthOfOperandList < 0)
    {
      _result2 = registerAtrying;
      return true;
    }

    long target = 0;
    for (int d = 0; d < 8; d++)
    {
      _registerA = registerAtrying * 8 | (uint)d;
      int i = 0;
      while (i < _commandList.Count)
      {
        bool hit5 = false;
        switch (_commandList[i])
        {
          case 0:
            AdvRule(i);
            break;
          case 1:
            _registerB ^= _commandList[i + 1];
            break;
          case 2:
            BstRule(i);
            break;
          case 3:
            i = JnzRule(i);
            break;
          case 4:
            _registerB ^= _registerC;
            break;
          case 5:
            target = OutRule(i);
            hit5 = true;
            break;
          case 6:
            BdvRule(i);
            break;
          case 7:
            CdvRule(i);
            break;
        }

        if (hit5) break;

        i += 2;
      }

      if (target == _commandList[lengthOfOperandList] &&
          CalculateRegisterA(lengthOfOperandList - 1, registerAtrying * 8 | (uint)d))
      {
        return true;
      }
    }

    return false;
  }
  private static void CdvRule(int i)
  {

    _registerC = _commandList[i + 1] switch
    {
      <= 3 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _commandList[i + 1]))),
      4 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerA))),
      5 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerB))),
      6 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerC))),
      _ => _registerC
    };
  }
  private static void BdvRule(int i)
  {

    _registerB = _commandList[i + 1] switch
    {
      <= 3 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _commandList[i + 1]))),
      4 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerA))),
      5 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerB))),
      6 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerC))),
      _ => _registerB
    };
  }
  private static long OutRule(int i)
  {

    long target = _commandList[i + 1] switch
    {
      <= 3 => _commandList[i + 1] % 8,
      4 => _registerA % 8,
      5 => _registerB % 8,
      6 => _registerC % 8,
      _ => 0
    };
    return target;
  }
  private static int JnzRule(int i)
  {

    i = _registerA != 0 ?
      _commandList[i + 1] - 2 :
      i;
    return i;
  }
  private static void BstRule(int i)
  {

    _registerB = _commandList[i + 1] switch
    {
      <= 3 => _commandList[i + 1] % 8,
      4 => _registerA % 8,
      5 => _registerB % 8,
      6 => _registerC % 8,
      _ => _registerB
    };
  }
  private static void AdvRule(int i)
  {

    _registerA = _commandList[i + 1] switch
    {
      <= 3 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _commandList[i + 1]))),
      4 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerA))),
      5 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerB))),
      6 => Convert.ToInt64(Math.Floor(_registerA / Math.Pow(2, _registerC))),
      _ => _registerA
    };
  }
  private static string RunProcess(List<int> programIn, long registerA, long registerB, long registerC)
  {
    int instrPtr = 0;

    StringBuilder programOut = new();

    while (instrPtr < programIn.Count)
    {
      int opcode = programIn[instrPtr];
      int operand = programIn[instrPtr + 1];
      long combo = CalculateCombo(operand, registerA, registerB, registerC);

      (long action, long output) = ProcessOpCode(ref registerA, ref registerB, ref registerC, opcode, combo, operand);
      switch (action)
      {
        case -1:
          instrPtr = (int)output;
          continue;
        case -2:
          programOut.Append($"{output},");
          break;
      }

      instrPtr += 2;
    }

    return programOut.ToString();
  }

  private static (long, long) ProcessOpCode(ref long registerA,
    ref long registerB,
    ref long registerC,
    int opcode,
    long combo,
    int operand)
  {
    return opcode switch
    {
      0 => (registerA /= (int)Math.Pow(2, combo), 0),
      1 => (registerB ^= operand, 0),
      2 => (registerB = combo % 8, 0),
      3 => registerA != 0 ?
        (-1, operand) :
        (0, 0),
      4 => (registerB ^= registerC, 0),
      5 => (-2, combo % 8),
      6 => (registerB = registerA / (int)Math.Pow(2, combo), 0),
      7 => (registerC = registerA / (int)Math.Pow(2, combo), 0),
      _ => (0, 0)
    };
  }

  private static long CalculateCombo(int operand, long registerA, long registerB, long registerC)
  {
    return operand switch
    {
      0 or 1 or 2 or 3 => operand,
      4 => registerA,
      5 => registerB,
      6 => registerC,
      _ => throw new Exception("Invalid operand")
    };
  }
}