
using System;
using NUnit.Framework;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Test.Gaming.Diceroller.DomainObjects
{
  [TestFixture]
  public class TestNumberOfRolls
  {
    [Test]
    public void TestLongGroup()
    {
      string diceSpec;
      DiceSpecification spec;
      
      diceSpec = "5#4d6+3-1d4";
      spec = new DiceSpecification(diceSpec);
      Assert.AreEqual(3, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual(diceSpec, spec.ToString(), "Dice string");
      Assert.AreEqual(5, spec.NumberOfRolls, "Number of rolls");
    }
    
    [Test]
    public void TestBrackets1()
    {
      string diceSpec;
      DiceSpecification spec;
      
      diceSpec = "3#(2d6*2)+1d6+3+(8/1d2)";
      spec = new DiceSpecification(diceSpec);
      Assert.AreEqual(4, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual(diceSpec, spec.ToString(), "Dice string");
      Assert.AreEqual(3, spec.NumberOfRolls, "Number of rolls");
    }
    
    [Test]
    public void TestBrackets2()
    {
      string diceSpec;
      DiceSpecification spec;
      
      diceSpec = "10#3d6-(3*(2d6+1))+100";
      spec = new DiceSpecification(diceSpec);
      Assert.AreEqual(3, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual(diceSpec, spec.ToString(), "Dice string");
      Assert.AreEqual(10, spec.NumberOfRolls, "Number of rolls");
    }
    
    [Test]
    public void TestTooManyOpenBrackets()
    {
      string diceSpec;
      DiceSpecification spec;
      
      diceSpec = "1#((2d4*2)-10";
      spec = new DiceSpecification(diceSpec);
      Assert.AreEqual(1, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual("((2d4*2)-10)", spec.ToString(), "Dice string");
    }
    
    [Test]
    public void TestTooManyCloseBrackets1()
    {
      string diceSpec;
      DiceSpecification spec;
      
      diceSpec = "6#1d%+(4d6x3-2))";
      spec = new DiceSpecification(diceSpec);
      Assert.AreEqual(2, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual("6#1d100+(4d6*3-2)", spec.ToString(), "Dice string");
      Assert.AreEqual(6, spec.NumberOfRolls, "Number of rolls");
    }
    
    [Test]
    public void TestTooManyCloseBrackets2()
    {
      string diceSpec;
      DiceSpecification spec;
      
      diceSpec = "9#1d%+(4d6x3-2))+2d6";
      spec = new DiceSpecification(diceSpec);
      Assert.AreEqual(3, spec.GetDice().Groups.Count, "Number of groups");
      Assert.AreEqual("9#1d100+(4d6*3-2)+2d6", spec.ToString(), "Dice string");
      Assert.AreEqual(9, spec.NumberOfRolls, "Number of rolls");
    }
  }
}
