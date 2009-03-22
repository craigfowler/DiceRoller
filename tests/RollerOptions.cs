/*
 * RollerOptions Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using NUnit.Framework;
using CraigFowler.Diceroller;

namespace Test.CraigFowler.Diceroller
{
  [Ignore("This test isn't finished yet")]
  [TestFixture]
  public class RollerOptions
  {
    /* Things I need to test here:
     * Rounding to an integer:
     * * Always up
     * * Always down
     * * Midpoint away from zero
     * * Midpoint to even
     * 
     * ---
     * This is where we're up to
     * 
     * Rounding to a decimal
     * * Always up
     * * Always down
     * * Midpoint away from zero
     * * Midpoint to even
     * 
     * Upper bounds
     * Lower bounds
     * 
     * Rounding at every step along the way and also rounding just the final
     * result.
     * 
     * Try setting some of these options on individual dice groups as well
     * as the whole spec.
     * 
     * Exploding dice:
     * * None (the default)
     * * Add (add the new roll onto the last roll)
     * * RollAgain (another roll entirely)
     * These are going to be quite hard to test, because in order for them to
     * explode, I am going to have to deal with random numbers.  In this case
     * I shall output this stuff to the commandline and have to check manually.
     * 
     * Never allow static numbers to explode, regardless.  Also the dice
     * mustn't be allowed to explode if the exploding threshold is
     * less-than-or-equal-to the minimum result of the roll.
     */
    
    private CoreDiceRoller roller;
    
    [SetUp]
    public void CreateRoller()
    {
      roller = new CoreDiceRoller();
    }
    
    [Test]
    public void RoundUpInteger()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.AlwaysUp;
      Assert.AreEqual(new decimal[] {3m},
                      roller.Roll("7/3"),
                      "Real result = 2 1/3");
      Assert.AreEqual(new decimal[] {3m},
                      roller.Roll("8/3"),
                      "Real result = 2 2/3");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
    }
    
    [Test]
    public void RoundDownInteger()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.AlwaysDown;
      Assert.AreEqual(new decimal[] {2m},
                      roller.Roll("7/3"),
                      "Real result = 2 1/3");
      Assert.AreEqual(new decimal[] {2m},
                      roller.Roll("8/3"),
                      "Real result 2 2/3");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
    }
    
    [Test]
    public void RoundAwayFromZeroInteger()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.MidpointAwayFromZero;
      Assert.AreEqual(new decimal[] {2m},
                      roller.Roll("7/3"),
                      "Real result = 2 1/3");
      Assert.AreEqual(new decimal[] {3m},
                      roller.Roll("8/3"),
                      "Real result = 2 2/3");
      Assert.AreEqual(new decimal[] {3m},
                      roller.Roll("5/2"),
                      "Real result = 2.5");
      Assert.AreEqual(new decimal[] {-2m},
                      roller.Roll("-7/3"),
                      "Real result = -2 1/3");
      Assert.AreEqual(new decimal[] {-3m},
                      roller.Roll("-8/3"),
                      "Real result = 2 2/3");
      Assert.AreEqual(new decimal[] {-3m},
                      roller.Roll("-5/2"),
                      "Real result = -2.5");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
    }
    
    [Test]
    public void RoundToEvenInteger()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.MidpointToEven;
      Assert.AreEqual(new decimal[] {2m},
                      roller.Roll("7/3"),
                      "Real result = 2 1/3");
      Assert.AreEqual(new decimal[] {3m},
                      roller.Roll("8/3"),
                      "Real result = 2 2/3");
      Assert.AreEqual(new decimal[] {2m},
                      roller.Roll("5/2"),
                      "Real result = 2.5");
      Assert.AreEqual(new decimal[] {-2m},
                      roller.Roll("-7/3"),
                      "Real result = -2 1/3");
      Assert.AreEqual(new decimal[] {-3m},
                      roller.Roll("-8/3"),
                      "Real result = 2 2/3");
      Assert.AreEqual(new decimal[] {-2m},
                      roller.Roll("-5/2"),
                      "Real result = -2.5");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
    }
  }
}
