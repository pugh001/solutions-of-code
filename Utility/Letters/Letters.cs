namespace Utility;

public static class Letters
{
  public static int CountVowels(string letters)
  {
    if (string.IsNullOrEmpty(letters))
      return 0;

    const string vowels = "aeiou";
    return letters.Count(c => vowels.Contains(c));
  }

  public static bool IsRepeatedLetter(string letters)
  {
    for (int i = 0; i < letters.Length - 1; i++)
    {
      if (letters[i] == letters[i + 1]) return true;
    }

    return false;
  }
  public static bool ContainsNoneOverlappingPair(string input)
  {
    if (string.IsNullOrEmpty(input) || input.Length < 4)
      return false;

    var seenPairs = new HashSet<string>();

    for (int i = 0; i < input.Length - 1; i++)
    {
      string pair = input.Substring(i, 2);

      if (seenPairs.Contains(pair))
        return true;

      // Add the current pair to the set, but exclude the pair starting at the previous index
      if (i > 0)
        seenPairs.Add(input.Substring(i - 1, 2));
    }

    return false;
  }
}