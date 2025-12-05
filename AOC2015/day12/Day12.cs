using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Utility;

namespace AOC2015;

public class Day12
{
  private long _sumPart2 = 0;

  private long _sumPart1 = 0;
  public (string, string) Process(string input)
  {

    string? data = SetupInputFile.OpenFile(input).First();


    _sumPart1 = ExtractSum(data);
    _sumPart2 = ExtractSum(data, "ignore");

    return (_sumPart1.ToString(), _sumPart2.ToString());
  }

  private static long ExtractSum(string json, string skip = "donotskip")
  {
    long numbers = 0;

    using var doc = JsonDocument.Parse(json);
    numbers = Traverse(doc.RootElement, numbers, skip);

    return numbers;
  }

  private static long Traverse(JsonElement element, long numbers, string skip)
  {
    switch (element.ValueKind)
    {
      case JsonValueKind.Number:
        numbers += long.Parse(element.ToString());
        break;

      case JsonValueKind.Array:
        foreach (var item in element.EnumerateArray())
          numbers = Traverse(item, numbers, skip);
        break;

      case JsonValueKind.Object:
        // Check if any property value equals the skip value
        foreach (var prop in element.EnumerateObject())
        {
          if (prop.Value.ValueKind == JsonValueKind.String && 
              prop.Value.GetString() == skip)
          {
            // Skip this entire object if any property value matches skip
            return numbers;
          }
        }
        
        // If no property value matches skip, traverse all properties
        foreach (var prop in element.EnumerateObject())
        {
          numbers = Traverse(prop.Value, numbers, skip);
        }
        break;

      // strings, null, true/false are ignored on purpose
    }

    return numbers;
  }
}
