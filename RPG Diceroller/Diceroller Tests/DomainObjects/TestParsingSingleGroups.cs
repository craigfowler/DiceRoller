
using System;
using NUnit.Framework;
using CraigFowler.Test.Gaming.Diceroller;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Test.Gaming.Diceroller.DomainObjects
{
  [TestFixture]
  public class TestParsingSingleGroups
  {
    #region testCode
    [Test]
    public void PositiveInteger()
    {
      DiceSpecification spec = new DiceSpecification("10");
      
      Assert.AreEqual(0, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual(10, spec.GetDice().Dice, "Number of dice");
      Assert.AreEqual(1, spec.GetDice().Sides, "Sides per die");
      Assert.AreEqual(DiceGroupOperator.Add,
                      spec.GetDice().Operator,
                      "Operator");
      Assert.AreEqual("10", spec.ToString(), "Correct string parsed");
    }
    
    [Test]
    public void NegativeInteger()
    {
      DiceSpecification spec = new DiceSpecification("-6");
      
      Assert.AreEqual(0, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual(6, spec.GetDice().Dice, "Number of dice");
      Assert.AreEqual(1, spec.GetDice().Sides, "Sides per die");
      Assert.AreEqual(DiceGroupOperator.Subtract,
                      spec.GetDice().Operator,
                      "Operator");
      Assert.AreEqual("-6", spec.ToString(), "Correct string parsed");
    }
    
    [Test]
    public void PositiveDiceroll()
    {
      DiceSpecification spec = new DiceSpecification("3d6");
      
      Assert.AreEqual(0, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual(3, spec.GetDice().Dice, "Number of dice");
      Assert.AreEqual(6, spec.GetDice().Sides, "Sides per die");
      Assert.AreEqual(DiceGroupOperator.Add,
                      spec.GetDice().Operator,
                      "Operator");
      Assert.AreEqual("3d6", spec.ToString(), "Correct string parsed");
    }
    
    [Test]
    public void NegativeDiceroll()
    {
      DiceSpecification spec = new DiceSpecification("-2D4");
      
      Assert.AreEqual(0, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual(2, spec.GetDice().Dice, "Number of dice");
      Assert.AreEqual(4, spec.GetDice().Sides, "Sides per die");
      Assert.AreEqual(DiceGroupOperator.Subtract,
                      spec.GetDice().Operator,
                      "Operator");
      Assert.AreEqual("-2d4", spec.ToString(), "Correct string parsed");
    }
    
    [Test]
    public void InvalidDiceSpec()
    {
      DiceSpecification spec = new DiceSpecification("3e4");
      
      try
      {
        spec.GetDice();
        Assert.Fail("Should not reach this point due to an exception");
      }
      catch(FormatException ex)
      {
        Assert.AreEqual("Missing operator whilst parsing.",
                        ex.Message,
                        "Exception message");
      }
      
      Assert.AreEqual("[Invalid dice specification]",
                      spec.ToString(),
                      "Specification string");
    }
    
    [Test]
    public void MultiplicationAtStart()
    {
      DiceSpecification spec = new DiceSpecification("x3D4");
      
      try
      {
        spec.GetDice();
        Assert.Fail("Should not reach this point due to an exception");
      }
      catch(FormatException ex)
      {
        Assert.AreEqual("Dice string must not begin with multiplication or " +
                        "division.",
                        ex.Message,
                        "Exception message");
      }
      
      Assert.AreEqual("[Invalid dice specification]",
                      spec.ToString(),
                      "Specification string");
    }
    #endregion
  }
}
