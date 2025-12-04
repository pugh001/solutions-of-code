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
  public static bool DoesItContainInvalidLetters(string letters, string disallowed)
  {
    return !letters.Any(c => disallowed.Contains(c));
  }
  public static string AddLetterToString(string data)
  {
    char[] letters = data.ToCharArray();
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
    for (int i = 0; i <= newPassword.Length - requiredInSequence; i++)
    {
      bool isSequential = true;

      for (int j = 1; j < requiredInSequence; j++)
      {
        if (newPassword[i + j] == newPassword[i + j - 1] + 1)
          continue;

        isSequential = false;
        break;
      }

      if (isSequential)
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