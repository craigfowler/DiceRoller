
using System;
using NUnit.Framework;
using CraigFowler.Gaming.Diceroller.Plugins;

namespace CraigFowler.Test.Gaming.Diceroller.Plugins
{
  [TestFixture]
  public class TestDnD3
  {
    [Test]
    public void Test4D6DropLowest()
    {
      DnD3e dice = new DnD3e();
      int[] statBlock;
      
      dice.RollingMethod = DnD3eRollingMethod.FourD6DropLowest;
      statBlock = dice.RollStats();
      Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}",
                        statBlock[0],
                        statBlock[1],
                        statBlock[2],
                        statBlock[3],
                        statBlock[4],
                        statBlock[5]);
    }
    
    [Test]
    public void Test3D6()
    {
      DnD3e dice = new DnD3e();
      int[] statBlock;
      
      dice.RollingMethod = DnD3eRollingMethod.ThreeD6;
      statBlock = dice.RollStats();
      Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}",
                        statBlock[0],
                        statBlock[1],
                        statBlock[2],
                        statBlock[3],
                        statBlock[4],
                        statBlock[5]);
    }
  }
}
