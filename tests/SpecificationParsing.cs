/*
 * SpecificationParsing Created on 21/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using NUnit.Framework;
using CraigFowler.Diceroller;

namespace Test.CraigFowler.Diceroller
{
  [TestFixture]
  public class SpecificationParsing
  {
    /* Things I need to test:
     * Several invalid dice specifications.  Can I think of any that would go
     * wrong and would totally prevent the diceroller from being able to do
     * its job?
     * 
     * Multiple rolls
     * 
     * * Check brackets
     * * Check operators
     * * Check that % (on its own) is used as a synonym for 100
     * * Check chained operators
     * * Check that whitespace is ignored
     * 
     * Actual cases to test (and how the test should behave)
     * * Too many opening brackets
     * > Any open brackets are implicitly closed at the end of the string
     * 
     * * Too many closing brackets
     * > Any extra ones are ignored/discarded
     * 
     * * Adjacent operation symbols
     * > Only the first operation encountered is used, the rest are discarded
     * 
     * * Multiplication/Division at the start of the string
     * > Throw an exception with an error message
     * 
     * * Number followed immediately by an open-bracket (without an operation)
     * > Throw an exception with an error message
     * 
     * * Invalid characters in the specification
     * > Discard them as well as the dice group they appear within
     * 
     * * Whitespace between numbers
     * > Discard it
     */
    
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
                                                      "32#3d8*2+20");
#endregion
    
    [Test]
    public void PlainNumber()
    {
      Assert.AreEqual(plainNumber.StringResult,
                      DiceSpecification.Parse(plainNumber.DiceSpecification));
    }
    
    [Test]
    public void AddedNumbers()
    {
      Assert.AreEqual(addedNumbers.StringResult,
                      DiceSpecification.Parse(addedNumbers.DiceSpecification));
    }
    
    [Test]
    public void ManyOperators()
    {
      Assert.AreEqual(manyOperators.StringResult,
                      DiceSpecification.Parse(manyOperators.DiceSpecification));
    }
    
    [Test]
    public void Brackets()
    {
      Assert.AreEqual(brackets.StringResult,
                      DiceSpecification.Parse(brackets.DiceSpecification));
    }
    
    [Test]
    public void Whitespace()
    {
      Assert.AreEqual(whitespace.StringResult,
                      DiceSpecification.Parse(whitespace.DiceSpecification));
    }
    
    [Test]
    public void NormalDiceRoll()
    {
      Assert.AreEqual(normalDiceRoll.StringResult,
                      DiceSpecification.Parse(normalDiceRoll.DiceSpecification));
    }
    
    [Test]
    public void MultipleDiceRoll()
    {
      Assert.AreEqual(multipleDiceRoll.StringResult,
                      DiceSpecification.Parse(multipleDiceRoll.DiceSpecification));
    }
    
    [Test]
    public void DPercentage()
    {
      Assert.AreEqual(dPercentage.StringResult,
                      DiceSpecification.Parse(dPercentage.DiceSpecification));
    }
    
    [Test]
    public void OmitNumberOfDice()
    {
      Assert.AreEqual(omitNumberOfDice.StringResult,
                      DiceSpecification.Parse(omitNumberOfDice.DiceSpecification));
    }
    
    [Test]
    public void DoubleBrackets()
    {
      Assert.AreEqual(doubleBrackets.StringResult,
                      DiceSpecification.Parse(doubleBrackets.DiceSpecification));
    }
    
    [Test]
    public void TooManyOpeningBrackets()
    {
      Assert.AreEqual(tooManyOpeningBrackets.StringResult,
                      DiceSpecification.Parse(tooManyOpeningBrackets.DiceSpecification));
    }
    
    [Test]
    public void TooManyClosingBrackets()
    {
      Assert.AreEqual(tooManyClosingBrackets.StringResult,
                      DiceSpecification.Parse(tooManyClosingBrackets.DiceSpecification));
    }
    
    [Test]
    public void AdjacentOperations()
    {
      Assert.AreEqual(adjacentOperations.StringResult,
                      DiceSpecification.Parse(adjacentOperations.DiceSpecification));
    }
    
    [ExpectedException(typeof(FormatException),
      "Dice string must not begin with multiplication or division")]
    [Test]
    public void LeadingMultiplication()
    {
      DiceSpecification.Parse(leadingMultiplication.DiceSpecification);
    }
    
    [ExpectedException(typeof(FormatException),
      "Missing operation before opening parenthesis")]
    [Test]
    public void MissingOperation()
    {
      DiceSpecification.Parse(missingOperation.DiceSpecification);
    }
    
    [Test]
    public void InvalidCharacters()
    {
      Assert.AreEqual(invalidCharacters.StringResult,
                      DiceSpecification.Parse(invalidCharacters.DiceSpecification));
    }
    
    [Test]
    public void WhitespaceInNumbers()
    {
      Assert.AreEqual(whitespaceInNumbers.StringResult,
                      DiceSpecification.Parse(whitespaceInNumbers.DiceSpecification));
    }
    
    [Test]
    public void MultipleRolls()
    {
      Assert.AreEqual(mutlipleRolls.StringResult,
                      DiceSpecification.Parse(mutlipleRolls.DiceSpecification));
    }
    
    [Test]
    public void MultipleRollsComplex()
    {
      Assert.AreEqual(multipleRollsComplex.StringResult,
                      DiceSpecification.Parse(multipleRollsComplex.DiceSpecification));
    }
  }
}
