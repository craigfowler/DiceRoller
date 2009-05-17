/*
 * Stats Created on 10/04/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using CraigFowler.Diceroller;
using NUnit.Framework;

namespace CraigFowler.Test.Diceroller
{
  [TestFixture]
  public class Stats
  {
#region testDefinitions
    private readonly TestSpec
      plainNumber                      = new TestSpec("1", "1"),
      addedNumbers                     = new TestSpec("2+3+4", "2+3+4"),
      manyOperators                    = new TestSpec("2x3-4/2","2*3-4/2"),
      brackets                         = new TestSpec("(1+3)*2", "(1+3)*2"),
      whitespace                       = new TestSpec("  8/4  + 2", "8/4+2"),
      normalDiceRoll                   = new TestSpec("2d6", "2d6"),
      multipleDiceRoll                 = new TestSpec("1d20-1d4x2",
                                                      "1d20-1d4*2"),
      dPercentage                      = new TestSpec("1d%", "1d100"),
      omitNumberOfDice                 = new TestSpec("d6+d4", "1d6+1d4"),
      doubleBrackets                   = new TestSpec("2*((3d6+4)*2-3)",
                                                      "2*((3d6+4)*2-3)"),
      tooManyOpeningBrackets           = new TestSpec("(20+2", "(20+2)"),
      tooManyClosingBrackets           = new TestSpec("4d6+1)", "4d6+1"),
      adjacentOperations               = new TestSpec("20-*2d6", "20-2d6"),
      leadingMultiplication            = new TestSpec("x20"),
      missingOperation                 = new TestSpec("4+3(4-2)"),
      invalidCharacters                = new TestSpec("2ad6+34", "34"),
      whitespaceInNumbers              = new TestSpec("3 4+1d4", "34+1d4"),
      mutlipleRolls                    = new TestSpec("4#2d6", "4#2d6"),
      multipleRollsComplex             = new TestSpec("32#3d8*2+20",
                                                      "32#3d8*2+20"),
      lotsOfBrackets                   = new TestSpec("5+(((5-4)*2))+9",
                                                      "5+(((5-4)*2))+9"),
      redundantBrackets                = new TestSpec("4+(2d6)+(9)", "4+2d6+9"),
      invalidSpecTwoDs                 = new TestSpec("10d6d8");
#endregion
    
    private CoreDiceRoller roller;
    
    [SetUp]
    public void CreateRoller()
    {
      roller = new CoreDiceRoller();
    }
    
    [Test]
    public void PlainNumber()
    {
      Assert.AreEqual(plainNumber.NumericResult,
                      roller.Minimum(plainNumber.DiceSpecification));
    }
  }
}
