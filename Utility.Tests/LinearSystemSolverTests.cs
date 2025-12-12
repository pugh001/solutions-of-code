using System.Collections.Generic;
using Utility;
using Xunit;

namespace Utility.Tests;

public class LinearSystemSolverTests
{
  [Fact]
  public void Solve_SimpleSystem_ReturnsMinimalPresses()
  {
    // 2 equations, 2 variables
    // button 0 affects eq0 and eq1; button1 affects eq1 only
    var goal = new List<int>{1,2};
    var buttonEffects = new List<List<int>>
    {
      new List<int>{0,1},
      new List<int>{1}
    };

    var solver = new LinearSystemSolver(goal, buttonEffects);
    int result = solver.Solve();

    // One valid solution is: button0 = 1, button1 = 1 -> eq0=1, eq1=2 total presses=2
    Assert.Equal(2, result);
  }

  [Fact]
  public void Solve_ImpossibleSystem_ReturnsZero()
  {
    // equation demands 1, but no buttons affect equation
    var goal = new List<int>{1};
    var buttonEffects = new List<List<int>>
    {
      new List<int>()
    };

    var solver = new LinearSystemSolver(goal, buttonEffects);
    int result = solver.Solve();
    Assert.Equal(0, result);
  }
}
