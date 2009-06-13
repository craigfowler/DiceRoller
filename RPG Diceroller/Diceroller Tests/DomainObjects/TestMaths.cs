
using System;
using NUnit.Framework;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Test.Gaming.Diceroller.DomainObjects
{
  [TestFixture]
  public class TestMaths
  {
    #region constants
    private const string
      SPEC_ONE                = "2d6+4",
      SPEC_TWO                = "d%",
      SPEC_THREE              = "8d4-2",
      SPEC_FOUR               = "3d6+(2*6d8)",
      SPEC_FIVE               = "(2d4/2)+20-(5d6+1)";
    #endregion
    
    #region lowRolls
    [Test]
    public void TestLowestDiceRolls1()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_ONE);
      Assert.AreEqual(6,
                      spec.GetDice().GetValue(CalculationMethod.LowestDiceRolls),
                      "Correct minimum");
    }
    
    [Test]
    public void TestLowestDiceRolls2()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_TWO);
      Assert.AreEqual(1,
                      spec.GetDice().GetValue(CalculationMethod.LowestDiceRolls),
                      "Correct minimum");
    }
    
    [Test]
    public void TestLowestDiceRolls3()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_THREE);
      Assert.AreEqual(6,
                      spec.GetDice().GetValue(CalculationMethod.LowestDiceRolls),
                      "Correct minimum");
    }
    
    [Test]
    public void TestLowestDiceRolls4()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FOUR);
      Assert.AreEqual(15,
                      spec.GetDice().GetValue(CalculationMethod.LowestDiceRolls),
                      "Correct minimum");
    }
    
    [Test]
    public void TestLowestDiceRolls5()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FIVE);
      Assert.AreEqual(15,
                      spec.GetDice().GetValue(CalculationMethod.LowestDiceRolls),
                      "Correct minimum");
    }
    #endregion
    
    #region highRolls
    [Test]
    public void TestHighestDiceRolls1()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_ONE);
      Assert.AreEqual(16,
                      spec.GetDice().GetValue(CalculationMethod.HighestDiceRolls),
                      "Correct maximum");
    }
    
    [Test]
    public void TestHighestDiceRolls2()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_TWO);
      Assert.AreEqual(100 ,
                      spec.GetDice().GetValue(CalculationMethod.HighestDiceRolls),
                      "Correct maximum");
    }
    
    [Test]
    public void TestHighestDiceRolls3()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_THREE);
      Assert.AreEqual(30,
                      spec.GetDice().GetValue(CalculationMethod.HighestDiceRolls),
                      "Correct maximum");
    }
    
    [Test]
    public void TestHighestDiceRolls4()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FOUR);
      Assert.AreEqual(114,
                      spec.GetDice().GetValue(CalculationMethod.HighestDiceRolls),
                      "Correct maximum");
    }
    
    [Test]
    public void TestHighestDiceRolls5()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FIVE);
      Assert.AreEqual(-7,
                      spec.GetDice().GetValue(CalculationMethod.HighestDiceRolls),
                      "Correct maximum");
    }
    #endregion
    
    #region means
    [Test]
    public void TestMean1()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_ONE);
      Assert.AreEqual(11,
                      spec.GetDice().GetValue(CalculationMethod.Mean),
                      "Correct mean");
    }
    
    [Test]
    public void TestMean2()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_TWO);
      Assert.AreEqual(50.5m ,
                      spec.GetDice().GetValue(CalculationMethod.Mean),
                      "Correct mean");
    }
    
    [Test]
    public void TestMean3()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_THREE);
      Assert.AreEqual(18,
                      spec.GetDice().GetValue(CalculationMethod.Mean),
                      "Correct mean");
    }
    
    [Test]
    public void TestMean4()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FOUR);
      Assert.AreEqual(64.5m,
                      spec.GetDice().GetValue(CalculationMethod.Mean),
                      "Correct mean");
    }
    
    [Test]
    public void TestMean5()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FIVE);
      Assert.AreEqual(4,
                      spec.GetDice().GetValue(CalculationMethod.Mean),
                      "Correct mean");
    }
    #endregion
    
    #region minimums
    [Test]
    public void TestMinimum1()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_ONE);
      Assert.AreEqual(6,
                      spec.GetDice().GetValue(CalculationMethod.Minimum),
                      "Correct minimum");
    }
    
    [Test]
    public void TestMinimum2()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_TWO);
      Assert.AreEqual(1,
                      spec.GetDice().GetValue(CalculationMethod.Minimum),
                      "Correct minimum");
    }
    
    [Test]
    public void TestMinimum3()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_THREE);
      Assert.AreEqual(6,
                      spec.GetDice().GetValue(CalculationMethod.Minimum),
                      "Correct minimum");
    }
    
    [Test]
    public void TestMinimum4()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FOUR);
      Assert.AreEqual(15,
                      spec.GetDice().GetValue(CalculationMethod.Minimum),
                      "Correct minimum");
    }
    
    [Test]
    public void TestMinimum5()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FIVE);
      Assert.AreEqual(-10,
                      spec.GetDice().GetValue(CalculationMethod.Minimum),
                      "Correct minimum");
    }
    #endregion
    
    #region maximums
    [Test]
    public void TestMaximum1()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_ONE);
      Assert.AreEqual(16,
                      spec.GetDice().GetValue(CalculationMethod.Maximum),
                      "Correct maximum");
    }
    
    [Test]
    public void TestMaximum2()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_TWO);
      Assert.AreEqual(100 ,
                      spec.GetDice().GetValue(CalculationMethod.Maximum),
                      "Correct maximum");
    }
    
    [Test]
    public void TestMaximum3()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_THREE);
      Assert.AreEqual(30,
                      spec.GetDice().GetValue(CalculationMethod.Maximum),
                      "Correct maximum");
    }
    
    [Test]
    public void TestMaximum4()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FOUR);
      Assert.AreEqual(114,
                      spec.GetDice().GetValue(CalculationMethod.Maximum),
                      "Correct maximum");
    }
    
    [Test]
    public void TestMaximum5()
    {
      DiceSpecification spec = new DiceSpecification(SPEC_FIVE);
      Assert.AreEqual(18,
                      spec.GetDice().GetValue(CalculationMethod.Maximum),
                      "Correct maximum");
    }
    #endregion
  }
}
