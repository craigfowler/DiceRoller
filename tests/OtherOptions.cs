/*
 * OtherOptions Created on 22/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using NUnit.Framework;
using CraigFowler.Diceroller;

namespace Test.CraigFowler.Diceroller
{
  [TestFixture]
  public class OtherOptions
  {
    private CoreDiceRoller roller;
    
    [SetUp]
    public void CreateRoller()
    {
      roller = new CoreDiceRoller();
    }
    
    [Test]
    public void ExplodingAddOn()
    {
      roller.Options.ExplodingThreshold = 4;
      roller.Options.ExplodingStyle = ExplodingStyle.Add;
      
      for(int i = 0; i < 20; i++)
      {
        Console.WriteLine("Add-on-exploding: i='{0,2}', result='{1,2}'",
                          (i + 1),
                          roller.Roll("1d4")[0]);
      }
      
      roller.Options.ExplodingThreshold = null;
      roller.Options.ExplodingStyle = ExplodingStyle.None;
    }
    
    [Test]
    public void ExplodingRollAgain()
    {
      roller.Options.ExplodingThreshold = 4;
      roller.Options.ExplodingStyle = ExplodingStyle.RollAgain;
      
      for(int i = 0; i < 20; i++)
      {
        Console.WriteLine("Add-on-exploding: i='{0,2}', results='{1,2}'",
                          (i + 1),
                          roller.Roll("1d4").Length);
      }
      
      roller.Options.ExplodingThreshold = null;
      roller.Options.ExplodingStyle = ExplodingStyle.None;
    }
    
    [Test]
    public void DontExplodeNumbers()
    {
      roller.Options.ExplodingThreshold = 4;
      roller.Options.ExplodingStyle = ExplodingStyle.Add;
      
      for(int i = 0; i < 20; i++)
      {
        Assert.AreEqual(new decimal[] {6m},
                        roller.Roll("5+1"),
                        "Check that the numbers never explode");
      }
      
      roller.Options.ExplodingThreshold = null;
      roller.Options.ExplodingStyle = ExplodingStyle.None;
    }
    
    [Test]
    public void DontExplodeIfThresholdIsTooLow()
    {
      roller.Options.ExplodingThreshold = 2;
      roller.Options.ExplodingStyle = ExplodingStyle.Add;
      roller.Options.LowerBound = 2;
      
      for(int i = 0; i < 20; i++)
      {
        Assert.Greater(roller.Roll("1d6")[0],
                       1m,
                       "Must be greater than one (lower bound is 2)");
        Assert.Less(roller.Roll("1d6")[0],
                    7m,
                    "Must be less than 7 (it's a d6)");
      }
      
      roller.Options.ExplodingThreshold = null;
      roller.Options.LowerBound = null;
      roller.Options.ExplodingStyle = ExplodingStyle.None;
    }
    
    [Test]
    public void OptionsOnIndividualDiceGroups()
    {
      DiceSpecification spec = DiceSpecification.Parse("18/7+3/7");
      spec.Groups[0].Options.Rounding.RoundingType = RoundingMethod.AlwaysDown;
      spec.Groups[2].Options.Rounding.RoundingType = RoundingMethod.AlwaysUp;
      
      Assert.AreEqual(new decimal[] {3m},
                      roller.Roll(spec),
                      "Rounding options applied to groups");
    }
  }
}
