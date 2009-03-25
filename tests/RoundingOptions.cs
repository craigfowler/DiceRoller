/*
 * RollerOptions Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using NUnit.Framework;
using CraigFowler.Diceroller;

namespace Test.CraigFowler.Diceroller
{
  [TestFixture]
  public class RoundingTests
  {
    /* Things I need to test here:
     * Rounding to an integer:
     * * Always up
     * * Always down
     * * Midpoint away from zero
     * * Midpoint to even
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
    
    [Test]
    public void RoundUpDecimal()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.AlwaysUp;
      roller.Options.Rounding.RoundingDigits = 2;
      Assert.AreEqual(new decimal[] {0.24m},
                      roller.Roll("(7/3)/10"),
                      "Real result = 0.2333");
      Assert.AreEqual(new decimal[] {0.27m},
                      roller.Roll("(8/3)/10"),
                      "Real result = 0.2666");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
      roller.Options.Rounding.RoundingDigits = 0;
    }
    
    [Test]
    public void RoundDownDecimal()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.AlwaysDown;
      roller.Options.Rounding.RoundingDigits = 2;
      Assert.AreEqual(new decimal[] {0.23m},
                      roller.Roll("(7/3)/10"),
                      "Real result = 0.2333");
      Assert.AreEqual(new decimal[] {0.26m},
                      roller.Roll("(8/3)/10"),
                      "Real result = 0.2666");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
      roller.Options.Rounding.RoundingDigits = 0;
    }
    
    [Test]
    public void RoundAwayFromZeroDecimal()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.MidpointAwayFromZero;
      roller.Options.Rounding.RoundingDigits = 2;
      Assert.AreEqual(new decimal[] {0.23m},
                      roller.Roll("(7/3)/10"),
                      "Real result = 0.2333");
      Assert.AreEqual(new decimal[] {0.27m},
                      roller.Roll("(8/3)/10"),
                      "Real result = 0.2666");
      Assert.AreEqual(new decimal[] {0.03m},
                      roller.Roll("(5/2)/100"),
                      "Real result = 0.025");
      Assert.AreEqual(new decimal[] {-0.23m},
                      roller.Roll("(-7/3)/10"),
                      "Real result = -0.2333");
      Assert.AreEqual(new decimal[] {-0.27m},
                      roller.Roll("(-8/3)/10"),
                      "Real result = -0.2666");
      Assert.AreEqual(new decimal[] {-0.03m},
                      roller.Roll("(-5/2)/100"),
                      "Real result = -0.025");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
      roller.Options.Rounding.RoundingDigits = 0;
    }
    
    [Test]
    public void RoundToEvenDecimal()
    {
      roller.Options.Rounding.RoundingType = RoundingMethod.MidpointToEven;
      roller.Options.Rounding.RoundingDigits = 2;
      Assert.AreEqual(new decimal[] {0.23m},
                      roller.Roll("(7/3)/10"),
                      "Real result = 0.2333");
      Assert.AreEqual(new decimal[] {0.27m},
                      roller.Roll("(8/3)/10"),
                      "Real result = 0.2666");
      Assert.AreEqual(new decimal[] {0.02m},
                      roller.Roll("(5/2)/100"),
                      "Real result = 0.025");
      Assert.AreEqual(new decimal[] {-0.23m},
                      roller.Roll("(-7/3)/10"),
                      "Real result = -0.2333");
      Assert.AreEqual(new decimal[] {-0.27m},
                      roller.Roll("(-8/3)/10"),
                      "Real result = -0.2666");
      Assert.AreEqual(new decimal[] {-0.02m},
                      roller.Roll("(-5/2)/100"),
                      "Real result = -0.025");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
      roller.Options.Rounding.RoundingDigits = 0;
    }
    
    [Test]
    public void LowerBound()
    {
      roller.Options.LowerBound = 3;
      
      for(int i = 0; i < 100; i++)
      {
        Assert.Greater(roller.Roll("1d6")[0], 2m, "Lower bound is three");
      }
      
      roller.Options.LowerBound = null;
    }
    
    [Test]
    public void UpperBound()
    {
      roller.Options.UpperBound = 5;
      
      for(int i = 0; i < 100; i++)
      {
        Assert.Less(roller.Roll("1d6")[0], 6m, "Lower bound is three");
      }
      
      roller.Options.UpperBound = null;
    }
    
    [Test]
    public void RoundEveryStep()
    {
      roller.Options.Rounding.RoundEveryStep = true;
      roller.Options.Rounding.RoundingDigits = 0;
      roller.Options.Rounding.RoundingType = RoundingMethod.AlwaysUp;
      Assert.AreEqual(new decimal[] {4m},
                      roller.Roll("2/7+9/5+10/12"),
                      "Round up every step");
      roller.Options.Rounding.RoundEveryStep = false;
      Assert.AreEqual(new decimal[] {3m},
                      roller.Roll("2/7+9/5+10/12"),
                      "Round up every step");
      roller.Options.Rounding.RoundingType = RoundingOptions.DEFAULT_ROUNDING;
    }
  }
}
