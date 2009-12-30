
using System;
using NUnit.Framework;
using CraigFowler.Gaming.Diceroller.Plugins;

namespace CraigFowler.Test.Gaming.Diceroller.Plugins
{
  [TestFixture]
  public class TestFudge
  {
    [Test]
    public void TestConstructor()
    {
      FudgeDice roller = new FudgeDice(4);
      string output;
      
      Assert.AreEqual(4, roller.NumberOfDice, "Number of dice");
      output = roller.ResultText;
      
      Assert.GreaterOrEqual(output.Length, 16);
      Assert.LessOrEqual(output.Length, 17);
      
      Console.WriteLine("Rolling {0} FUDGE dice.\n" +
                        "{1} positive, {2} negative, {3} neutral\n" +
                        "Text result: {4}\n" +
                        "Overall result: {5}",
                        roller.NumberOfDice,
                        roller.Positive,
                        roller.Negative,
                        roller.Neutral,
                        roller.ResultText,
                        roller.Overall);
    }
  }
}
