
using System;
using NUnit.Framework;
using CraigFowler.Gaming.Diceroller.Plugins;

namespace CraigFowler.Test.Gaming.Diceroller.Plugins
{
  [TestFixture]
  public class TestShadowrun4
  {
    [Test]
    public void TestRollingNormally()
    {
      Shadowrun4Glitch glitch;
      Shadowrun4 roller = new Shadowrun4(24);
      int successes;
      
      for(int i = 0; i < 200; i++)
      {
        successes = roller.Successes(out glitch);
        Console.WriteLine("{0} successes, glitch: {1}", successes, glitch.ToString());
      }
    }
    
    [Test]
    public void TestRollingWithEdge()
    {
      Shadowrun4Glitch glitch;
      Shadowrun4 roller = new Shadowrun4(24);
      roller.EdgeUsed = 1;
      int successes;
      
      for(int i = 0; i < 200; i++)
      {
        successes = roller.Successes(out glitch);
        Console.WriteLine("{0} successes, glitch: {1}", successes, glitch.ToString());
      }
    }
  }
}
