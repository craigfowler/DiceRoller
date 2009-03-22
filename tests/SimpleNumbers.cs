/*
 * SimpleNumbers Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using NUnit.Framework;
using CraigFowler.Diceroller;

namespace Test.CraigFowler.Diceroller
{
  [TestFixture]
  public class SimpleNumbers
  {
#region testDefinitions
    private readonly TestSpec
      simpleNumber                     = new TestSpec("43", 43m),
      manySimpleNumbers                = new TestSpec("2+8+2+10", 22m),
      negativeSimpleNumber             = new TestSpec("-9-4", -13m),
      multipliedNumbers                = new TestSpec("4x5", 20m),
      dividedNumbers                   = new TestSpec("6/2x8", 24m),
      multipliedNumbersWithSynonyms    = new TestSpec("2x2*2x5", 40m),
      simpleNumbersWithOrder           = new TestSpec("2x3+8/2-5", 5m),
      simpleNumbersWithWhitespace      = new TestSpec(" 10 - 6   * 2  ", -2m),
      simpleNumbersWithBrackets        = new TestSpec("4*(5-2)", 12m),
      invalidOperation                 = new TestSpec("3**4+8", 20m),
      invalidBrackets1                 = new TestSpec("4*(3+3", 24m),
      invalidBrackets2                 = new TestSpec("20/(7-2))", 4m),
      divideByZero                     = new TestSpec("30*2/(6-6)");
#endregion
    
    private CoreDiceRoller roller;
    
    [SetUp]
    public void CreateRoller()
    {
      roller = new CoreDiceRoller();
    }
    
    [Test]
    public void SimpleNumber()
    {
      Assert.AreEqual(simpleNumber.NumericResult,
                      roller.Roll(simpleNumber.DiceSpecification));
    }
    
    [Test]
    public void Addition()
    {
      Assert.AreEqual(manySimpleNumbers.NumericResult,
                      roller.Roll(manySimpleNumbers.DiceSpecification));
    }
    
    [Test]
    public void Subtraction()
    {
      Assert.AreEqual(negativeSimpleNumber.NumericResult,
                      roller.Roll(negativeSimpleNumber.DiceSpecification));
    }
    
    [Test]
    public void Multiplication()
    {
      Assert.AreEqual(multipliedNumbers.NumericResult,
                      roller.Roll(multipliedNumbers.DiceSpecification));
    }
    
    [Test]
    public void Division()
    {
      Assert.AreEqual(dividedNumbers.NumericResult,
                      roller.Roll(dividedNumbers.DiceSpecification));
    }
    
    [Test]
    public void MultiplicationSynonyms()
    {
      Assert.AreEqual(multipliedNumbersWithSynonyms.NumericResult,
                      roller.Roll(multipliedNumbersWithSynonyms.DiceSpecification));
    }
    
    [Test]
    public void OrderOfOperations()
    {
      Assert.AreEqual(simpleNumbersWithOrder.NumericResult,
                      roller.Roll(simpleNumbersWithOrder.DiceSpecification));
    }
    
    [Test]
    public void WhitespaceIgnored()
    {
      Assert.AreEqual(simpleNumbersWithWhitespace.NumericResult,
                      roller.Roll(simpleNumbersWithWhitespace.DiceSpecification));
    }
    
    [Test]
    public void Brackets()
    {
      Assert.AreEqual(simpleNumbersWithBrackets.NumericResult,
                      roller.Roll(simpleNumbersWithBrackets.DiceSpecification));
    }
    
    [Test]
    public void InvalidOperationRecovered()
    {
      Assert.AreEqual(invalidOperation.NumericResult,
                      roller.Roll(invalidOperation.DiceSpecification));
    }
    
    [Test]
    public void BracketsNotClosedRecovered()
    {
      Assert.AreEqual(invalidBrackets1.NumericResult,
                      roller.Roll(invalidBrackets1.DiceSpecification));
    }
    
    [Test]
    public void BracketsTooManyClosedRecovered()
    {
      Assert.AreEqual(invalidBrackets2.NumericResult,
                      roller.Roll(invalidBrackets2.DiceSpecification));
    }
    
    [Test]
    [ExpectedException(typeof(DivideByZeroException),
                       "Error: Division by zero!")]
    public void DivisionByZeroError()
    {
      Assert.AreEqual(divideByZero.NumericResult,
                      roller.Roll(divideByZero.DiceSpecification));
    }
  }
}
