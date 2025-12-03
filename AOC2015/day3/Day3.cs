using Utility;

namespace AOC2015;

public class Day3
{
  public (string, string) Process(string input)
  {
    string? data = SetupInputFile.OpenFile(input).First();
    var originalDelivery = new Dictionary<(int, int), int>();
    var roboDelivery = new Dictionary<(int, int), int>();

    DoYourMoves(data, originalDelivery);
    DoYourMovesTwo(data, roboDelivery);

    return (originalDelivery.Count.ToString(), roboDelivery.Count.ToString());
  }

  private static void DoYourMovesTwo(string data, Dictionary<(int, int), int> roboDelivery)
  {
    var santaHouse = (0, 0);
    var roboSantaHouse = (0, 0);
    roboDelivery[santaHouse] = 1;
    bool isSantaMove = true;
    foreach (var currentHouse in data.Select(move => isSantaMove ?
               santaHouse = santaHouse.MoveDirection(MoveDirections.CompassDirectionFromArrow(move)) :
               roboSantaHouse = roboSantaHouse.MoveDirection(MoveDirections.CompassDirectionFromArrow(move))))
    {
      roboDelivery.TryAdd(currentHouse, 1);
      isSantaMove = !isSantaMove;
    }
  }

  private static void DoYourMoves(string data, Dictionary<(int, int), int> originalDelivery)
  {
    var originalSantaHouse = (0, 0);
    originalDelivery[originalSantaHouse] = 1;

    foreach (char move in data)
    {
      originalSantaHouse = originalSantaHouse.MoveDirection(MoveDirections.CompassDirectionFromArrow(move));
      originalDelivery.TryAdd(originalSantaHouse, 1);
    }
  }
}