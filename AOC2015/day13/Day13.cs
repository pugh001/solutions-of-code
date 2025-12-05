using Utility;
using Utility.Algorithms;

namespace AOC2015;

public class Day13
{
  private long _sumPart1;
  private long _sumPart2;
  public (string, string) Process(string input)
  {
    var data = SetupInputFile.OpenFile(input);

    // Parse the happiness into a dictionary
    var happiness = new Dictionary<(string, string), int>();
    
    var personList = GetThePeople(data, happiness);
    _sumPart1  = Allhappiness(personList, happiness);

    AddPerson(personList,  happiness);
    personList.Add("me");
    _sumPart2 = Allhappiness(personList, happiness);

    return (_sumPart1.ToString(), _sumPart2.ToString());
  }
  private void AddPerson(List<string> personList, Dictionary<(string, string), int> happiness)
  {
    
    foreach (var person in personList)
    {
      happiness[("me", person)] = 0;
      happiness[(person, "me")] = 0;
    }
    
  }
  private static long Allhappiness(List<string> personList, Dictionary<(string, string), int> happiness)
  {

    var allPermutations = Algorithms.GetPermutations(personList);

    var allhappiness = new List<int>();

    foreach (var permutation in allPermutations)
    {
      int totalhappyValue = 0;
      bool validPath = true;
      int end = 0;
      int start = 0;
      // Calculate total happyValue for this route
      for (int i = 0; i < permutation.Count; i++)
        
      {
        //0-1,1-2,2-3,3-0
        //1-0,2-1,3-2,0-3
        start = i;
        end = i + 1;
        if (end > permutation.Count-1) { end = 0;}
        var key = (permutation[start], permutation[end]);
        if (happiness.ContainsKey(key))
        {
          totalhappyValue += happiness[key];
        }
        else
        {
          validPath = false;
          break;
        }
        key = (permutation[end],permutation[start]);
        if (happiness.ContainsKey(key))
        {
          totalhappyValue += happiness[key];
        }
        else
        {
          validPath = false;
          break;
        }
      }

      if (validPath)
      {
        allhappiness.Add(totalhappyValue);
      }
    }

    return allhappiness.Max();
  }
  private static List<string> GetThePeople(IEnumerable<string> data, Dictionary<(string, string), int> happiness)
  {

    var people = new HashSet<string>();

    foreach (string line in data)
    {
      string person1 = SplitValues(line, out string person2, out int happyValue);
      happiness[(person1, person2)] = happyValue;


      people.Add(person1);
      people.Add(person2);
    }

    var personList = people.ToList();
    return personList;
  }
  private static string SplitValues(string line, out string person2, out int happyValue)
  {

    // Parse lines like "Alice would gain 54 happiness units by sitting next to Bob."
    string[] parts = line.Split(" would ");
    string person1 = parts[0];
    string[] remaining = parts[1].Split(" happiness units by sitting next to ");
    person2 = remaining[1][..^1];
    string[] gainOrLose = remaining[0].Split(" ");
    int multipler = 1;
    if (gainOrLose[0] == "lose")
    {
      multipler = -1;
    }
    happyValue = int.Parse(gainOrLose[1]) * multipler;
    return person1;
  }
}