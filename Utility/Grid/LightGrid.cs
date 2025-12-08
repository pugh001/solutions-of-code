using System.Text.RegularExpressions;

namespace Utility.Grid;

/// <summary>
/// Utility for managing light grids with on/off and brightness operations
/// </summary>
public class LightGrid
{
  private readonly bool[,] _lights;
  private readonly int[,] _brightness;
  private readonly int _size;

  public LightGrid(int size = 1000)
  {
    _size = size;
    _lights = new bool[size, size];
    _brightness = new int[size, size];
  }

  public int Size => _size;

  /// <summary>
  /// Applies light operations based on instruction string
  /// </summary>
  public void ApplyInstruction(string instruction)
  {
    var (action, startX, startY, endX, endY) = ParseInstruction(instruction);
    ApplyOperation(startX, startY, endX, endY, action);
  }

  /// <summary>
  /// Applies multiple instructions
  /// </summary>
  public void ApplyInstructions(IEnumerable<string> instructions)
  {
    foreach (var instruction in instructions)
    {
      ApplyInstruction(instruction);
    }
  }

  /// <summary>
  /// Parses instruction string to extract action and coordinates
  /// </summary>
  public static (LightAction action, int startX, int startY, int endX, int endY) ParseInstruction(string instruction)
  {
    var regex = new Regex(@".* (\d+),(\d+) through (\d+),(\d+)");
    var match = regex.Match(instruction);
    
    if (!match.Success)
      throw new ArgumentException($"Invalid instruction format: {instruction}");

    int startX = int.Parse(match.Groups[1].Value);
    int startY = int.Parse(match.Groups[2].Value);
    int endX = int.Parse(match.Groups[3].Value);
    int endY = int.Parse(match.Groups[4].Value);

    var action = instruction switch
    {
      string s when s.StartsWith("turn on") => LightAction.TurnOn,
      string s when s.StartsWith("turn off") => LightAction.TurnOff,
      string s when s.StartsWith("toggle") => LightAction.Toggle,
      _ => throw new ArgumentException($"Unknown action in instruction: {instruction}")
    };

    return (action, startX, startY, endX, endY);
  }

  /// <summary>
  /// Applies operation to a rectangular region
  /// </summary>
  public void ApplyOperation(int startX, int startY, int endX, int endY, LightAction action)
  {
    for (int x = startX; x <= endX; x++)
    {
      for (int y = startY; y <= endY; y++)
      {
        ApplyToLight(x, y, action);
      }
    }
  }

  /// <summary>
  /// Applies action to a single light
  /// </summary>
  private void ApplyToLight(int x, int y, LightAction action)
  {
    if (x < 0 || x >= _size || y < 0 || y >= _size)
      return;

    switch (action)
    {
      case LightAction.TurnOn:
        _lights[x, y] = true;
        _brightness[x, y]++;
        break;
      case LightAction.TurnOff:
        _lights[x, y] = false;
        _brightness[x, y] = Math.Max(0, _brightness[x, y] - 1);
        break;
      case LightAction.Toggle:
        _lights[x, y] = !_lights[x, y];
        _brightness[x, y] += 2;
        break;
    }
  }

  /// <summary>
  /// Counts the number of lights that are on
  /// </summary>
  public int CountLightsOn()
  {
    int count = 0;
    for (int x = 0; x < _size; x++)
    {
      for (int y = 0; y < _size; y++)
      {
        if (_lights[x, y]) count++;
      }
    }
    return count;
  }

  /// <summary>
  /// Calculates total brightness of all lights
  /// </summary>
  public long CalculateTotalBrightness()
  {
    long total = 0;
    for (int x = 0; x < _size; x++)
    {
      for (int y = 0; y < _size; y++)
      {
        total += _brightness[x, y];
      }
    }
    return total;
  }

  /// <summary>
  /// Gets the state of a specific light
  /// </summary>
  public bool IsLightOn(int x, int y)
  {
    return x >= 0 && x < _size && y >= 0 && y < _size && _lights[x, y];
  }

  /// <summary>
  /// Gets the brightness of a specific light
  /// </summary>
  public int GetBrightness(int x, int y)
  {
    return x >= 0 && x < _size && y >= 0 && y < _size ? _brightness[x, y] : 0;
  }
}