namespace Utility;

public static class Letters
{
  // Pre-computed vowel lookup for O(1) access
  private static readonly HashSet<char> VowelSet = ['a', 'e', 'i', 'o', 'u'];
  
  // Cache for disallowed character sets to avoid repeated HashSet creation
  private static readonly Dictionary<string, HashSet<char>> DisallowedCache = new();
  
  public static int CountVowels(string letters)
  {
    int count = 0;
    if (string.IsNullOrEmpty(letters))
      return count;
    count += letters.Count(c => VowelSet.Contains(char.ToLower(c)));
    return count;
  }
  
  public static bool DoesItContainInvalidLetters(string letters, string disallowed)
  {
    if (string.IsNullOrEmpty(letters) || string.IsNullOrEmpty(disallowed))
      return true;

    // Cache disallowed character sets for repeated calls
    if (DisallowedCache.TryGetValue(disallowed, out var disallowedSet))
      return letters.All(c => !disallowedSet.Contains(c));

    disallowedSet = (HashSet<char>)[..disallowed];
    DisallowedCache[disallowed] = disallowedSet;

    // Early termination with O(1) lookup
    return letters.All(c => !disallowedSet.Contains(c));
  }
  public static string AddLetterToString(string data)
  {
    if (string.IsNullOrEmpty(data))
      return "a";

    // Use Span<char> for better performance with large strings
    Span<char> letters = stackalloc char[data.Length];
    data.AsSpan().CopyTo(letters);
    
    int index = letters.Length - 1;

    // Start from the rightmost character and increment
    while (index >= 0)
    {
      letters[index]++;

      // If the letter goes beyond 'z', wrap to 'a' and carry over
      if (letters[index] <= 'z')
      {
        break; // No carry needed, we're done
      }

      letters[index] = 'a';
      index--; // Move to the previous position to carry over
    }

    return new string(letters);
  }
  public static bool DoesItContainStraight(string newPassword, int requiredInSequence = 3)
  {
    if (string.IsNullOrEmpty(newPassword) || newPassword.Length < requiredInSequence)
      return false;

    // Single pass algorithm - check sequence as we go
    int currentSequenceLength = 1;
    
    for (int i = 1; i < newPassword.Length; i++)
    {
      if (newPassword[i] == newPassword[i - 1] + 1)
      {
        currentSequenceLength++;
        if (currentSequenceLength >= requiredInSequence)
          return true;
      }
      else
      {
        currentSequenceLength = 1;
      }
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

    // Use integer-based pairs for better performance than string allocation
    var seenPairs = new HashSet<int>();

    for (int i = 0; i < input.Length - 1; i++)
    {
      // Create a hash from two characters to avoid string allocation
      int pairHash = (input[i] << 8) | input[i + 1];

      if (seenPairs.Contains(pairHash))
        return true;

      // Add previous pair if it exists and we're not at the first position
      if (i <= 0)
        continue;

      int prevPairHash = (input[i - 1] << 8) | input[i];
      seenPairs.Add(prevPairHash);
    }

    return false;
  }
}
