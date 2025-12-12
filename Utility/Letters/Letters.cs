namespace Utility;

public static class Letters
{
  // Pre-computed vowel lookup for O(1) access
  private static readonly HashSet<char> VowelSet = ['a', 'e', 'i', 'o', 'u'];

  public static int CountVowels(string letters)
  {
    int count = 0;
    if (string.IsNullOrEmpty(letters))
      return count;

    foreach (char t in letters)
    {
      if (VowelSet.Contains(char.ToLower(t)))
        count++;
    }

    return count;
  }

  public static bool ContainsDisallowed(string s, HashSet<char> disallowed, out int position)
  {
    position = 0;
    foreach (char c in s)
    {
      // Normalize to lowercase for comparison
      char lower = (char)(c | 32);
      if (disallowed.Contains(lower))
        return true;

      position++;
    }

    return false;
  }
  public static string AddLetterToString(string data, HashSet<char> disallowed)
  {
    if (string.IsNullOrEmpty(data))
      return "a";

    Span<char> letters = stackalloc char[data.Length];
    data.AsSpan().CopyTo(letters);

    int index = letters.Length - 1;
    while (index >= 0)
    {
      letters[index]++;

      // Skip forbidden letters
      while (disallowed.Contains(letters[index]))
      {
        letters[index]++;
      }

      if (letters[index] <= 'z')
      {
        break;
      }

      letters[index] = 'a';
      index--;
    }

    return new string(letters);
  }
  public static bool DoesItContainStraight(string newPassword, int requiredInSequence = 3)
  {
    if (string.IsNullOrEmpty(newPassword) || newPassword.Length < requiredInSequence)
      return false;

    for (int i = 0; i <= newPassword.Length - requiredInSequence; i++)
    {
      bool isStraight = true;
      for (int j = 1; j < requiredInSequence; j++)
      {
        if (newPassword[i + j] == newPassword[i + j - 1] + 1)
          continue;

        isStraight = false;
        break;
      }

      if (isStraight)
        return true;
    }

    return false;
  }
  public static bool DoesItContainNoneOverlappingDifferentPairs(string inputString)
  {
    var foundPairs = new HashSet<char>();
    int i = 0;

    while (i < inputString.Length - 1)
    {
      // Check if current character matches the next one
      if (inputString[i] == inputString[i + 1])
      {
        foundPairs.Add(inputString[i]);
        i += 2; // Skip both characters of the pair to avoid overlapping

        // If we found 2 different pairs, we're done
        if (foundPairs.Count >= 2)
          return true;
      }
      else
      {
        i++; // Move to next character
      }
    }

    return false;
  }
  public static bool IsRepeatedLetter(string letters)
  {
    if (string.IsNullOrEmpty(letters) || letters.Length < 2)
      return false;

    // Single pass with early termination
    for (int i = 1; i < letters.Length; i++)
    {
      if (letters[i] == letters[i - 1])
        return true;
    }

    return false;
  }
  public static bool ContainsNoneOverlappingPair(string input)
  {
    if (string.IsNullOrEmpty(input) || input.Length < 4)
      return false;

    // Track all pairs and their last index for non-overlapping detection
    var pairIndices = new Dictionary<int, int>();
    for (int i = 0; i < input.Length - 1; i++)
    {
      int pairHash = input[i] << 8 | input[i + 1];
      if (pairIndices.TryGetValue(pairHash, out int lastIndex))
      {
        if (lastIndex < i - 1)
          return true;
      }
      else
      {
        pairIndices[pairHash] = i;
      }
    }

    return false;
  }
}