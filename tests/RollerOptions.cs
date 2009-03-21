/*
 * RollerOptions Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using NUnit.Framework;
using CraigFowler.Diceroller;

namespace Test.CraigFowler.Diceroller
{
  [Ignore("This test isn't ready yet")]
  [TestFixture]
  public class RollerOptions
  {
    /* Things I need to test here:
     * Rounding to an integer:
     * * Always up
     * * Always down
     * * Midpoint away from zero
     * * Midpoint to even
     * Rounding to a decimal
     * * Always up
     * * Always down
     * * Midpoint away from zero
     * * Midpoint to even
     * Upper bounds
     * Lower bounds
     * 
     * Exploding dice:
     * * None (the default)
     * * Add (add the new roll onto the last roll)
     * * RollAgain (another roll entirely)
     * These are going to be quite hard to test, because in order for them to
     * explode, I am going to have to deal with random numbers.  In this case
     * I shall output this stuff to the commandline and have to check manually.
     */
    
    [Test]
    public void TestCase()
    {
    }
  }
}
