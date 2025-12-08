using Utility;

namespace AOC2015;

public class Day13
{
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);
    var happiness = ParseHappinessData(data);
    var people = happiness.Keys.SelectMany(k => new[] { k.Item1, k.Item2 }).Distinct().ToList();

    var part1 = CalculateMaxHappiness(people, happiness);
    
    // Add myself with neutral happiness
    AddNeutralPerson("me", people, happiness);
    var part2 = CalculateMaxHappiness(people, happiness);

    return (part1.ToString(), part2.ToString());
  }

  private static Dictionary<(string, string), int> ParseHappinessData(IEnumerable<string> data)
  {
    var happiness = new Dictionary<(string, string), int>();
    
    foreach (string line in data)
    {
      var (person1, person2, value) = ParseHappinessLine(line);
      happiness[(person1, person2)] = value;
    }
    
    return happiness;
  }

  private static (string person1, string person2, int happiness) ParseHappinessLine(string line)
  {
    // Example: "Alice would gain 54 happiness units by sitting next to Bob."
    var parts = line.Split(" would ");
    string person1 = parts[0];
    
    var remaining = parts[1].Split(" happiness units by sitting next to ");
    string person2 = remaining[1].TrimEnd('.');
    
    var gainOrLose = remaining[0].Split(' ');
    int multiplier = gainOrLose[0] == "lose" ? -1 : 1;
    int value = int.Parse(gainOrLose[1]) * multiplier;
    
    return (person1, person2, value);
  }

  private static void AddNeutralPerson(string newPerson, List<string> people, Dictionary<(string, string), int> happiness)
  {
    foreach (var person in people)
    {
      happiness[(newPerson, person)] = 0;
      happiness[(person, newPerson)] = 0;
    }
    people.Add(newPerson);
  }

  private static long CalculateMaxHappiness(List<string> people, Dictionary<(string, string), int> happiness)
  {
    return Algorithms.GetPermutations(people)
      .Select(arrangement => CalculateArrangementHappiness(arrangement, happiness))
      .Max();
  }

  private static long CalculateArrangementHappiness(IList<string> arrangement, Dictionary<(string, string), int> happiness)
  {
    long totalHappiness = 0;
    int count = arrangement.Count;
    
    for (int i = 0; i < count; i++)
    {
      string current = arrangement[i];
      string next = arrangement[(i + 1) % count]; // Circular seating
      
      // Add happiness in both directions (person views their neighbors)
      totalHappiness += happiness.GetValueOrDefault((current, next), 0);
      totalHappiness += happiness.GetValueOrDefault((next, current), 0);
    }
    
    return totalHappiness;
  }
}
