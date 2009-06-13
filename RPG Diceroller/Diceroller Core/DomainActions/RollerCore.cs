
using System;
using System.Collections.Generic;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Gaming.Diceroller.DomainActions
{
  public static class RollerCore
  {
    #region fields
    private static Random randomiser;
    #endregion
    
    #region publicMethods
    
    /// <summary>
    /// Calculates the value of a dice group, using the specified calculation
    /// method.
    /// </summary>
    /// <param name="group">
    /// A <see cref="DiceGroup"/>, the dice group to calculate
    /// </param>
    /// <param name="baseMethod">
    /// A <see cref="CalculationMethod"/>, the calculation method to use
    /// </param>
    /// <returns>
    /// A <see cref="System.Decimal"/>, the result
    /// </returns>
    public static decimal CalculateValue(DiceGroup group,
                                         CalculationMethod baseMethod)
    {
      decimal groupValue;
      CalculationMethod method = getEffectiveMethod(baseMethod, group.Operator);
      List<DiceResult> groupResults;
      
      if(group.Groups != null && group.Groups.Count > 0)
      {
        groupResults = new List<DiceResult>();
        
        foreach(DiceGroup innerGroup in group.Groups)
        {
          groupResults.Add(new DiceResult(CalculateValue(innerGroup, method),
                                          innerGroup.Operator));
        }
        
        groupValue = getResultsTotal(groupResults);
      }
      else
      {
        groupValue = getDiceGroupValue(group.Sides, group.Dice, method);
      }
      
      return groupValue;
    }
    #endregion
    
    #region privateMethods
    
    /// <summary>
    /// Gets the effective calculation method for applying to a
    /// <see cref="DiceGroup"/>.  In most circumstances this simply returns the
    /// original calculation method however if we are looking for a minimum or
    /// maximum result (and the dice group's operator is subtraction or divison)
    /// then this method will 'invert' the calculation method.
    /// </summary>
    /// <param name="bm">
    /// A <see cref="CalculationMethod"/>, the base calculation method for this
    /// dice group.
    /// </param>
    /// <param name="oper">
    /// A <see cref="DiceGroupOperator"/>, the operator associated with the
    /// current dice group.
    /// </param>
    /// <returns>
    /// A <see cref="CalculationMethod"/> - the new effective calculation method
    /// to use.
    /// </returns>
    private static CalculationMethod getEffectiveMethod(CalculationMethod bm,
                                                        DiceGroupOperator oper)
    {
      CalculationMethod output;
      
      if(( oper == DiceGroupOperator.Subtract ||
           oper == DiceGroupOperator.Divide))
      {
        if(bm == CalculationMethod.Maximum)
        {
          output = CalculationMethod.Minimum;
        }
        else if(bm == CalculationMethod.Minimum)
        {
          output = CalculationMethod.Maximum;
        }
        else
        {
          output = bm;
        }
      }
      else
      {
        output = bm;
      }
      
      return output;
    }
    
    /// <summary>
    /// Combines the result of several dice groups into a single decimal.
    /// </summary>
    /// <param name="groupResults">
    /// A <see cref="List"/> of <see cref="DicreResult"/> objects, representing
    /// the results from the dice groups, along with their operators.
    /// </param>
    /// <param name="method">
    /// A <see cref="CalculationMethod"/> - the calculation method in use.
    /// </param>
    /// <returns>
    /// A <see cref="System.Decimal"/> - the result
    /// </returns>
    private static decimal getResultsTotal(List<DiceResult> groupResults)
    {
      if(groupResults[0].Operator == DiceGroupOperator.Subtract)
      {
        groupResults[0].Value = 0 - groupResults[0].Value;
      }
      
      for(int i = groupResults.Count - 1; i >= 1; i--)
      {
        if(groupResults[i].Operator == DiceGroupOperator.Divide)
        {
          groupResults[i - 1].Value =
            groupResults[i - 1].Value / groupResults[i].Value;
          groupResults.RemoveAt(i);
        }
        else if(groupResults[i].Operator == DiceGroupOperator.Multiply)
        {
          groupResults[i - 1].Value =
            groupResults[i - 1].Value * groupResults[i].Value;
          groupResults.RemoveAt(i);
        }
      }
      
      for(int i = groupResults.Count - 1; i >= 1; i--)
      {
        if(groupResults[i].Operator == DiceGroupOperator.Add)
        {
          groupResults[i - 1].Value =
            groupResults[i - 1].Value + groupResults[i].Value;
          groupResults.RemoveAt(i);
        }
        else if(groupResults[i].Operator == DiceGroupOperator.Subtract)
        {
          groupResults[i - 1].Value =
            groupResults[i - 1].Value - groupResults[i].Value;
          groupResults.RemoveAt(i);
        }
      }
      
      if(groupResults.Count != 1)
      {
        throw new InvalidOperationException("Impossible dice result count");
      }
      
      return groupResults[0].Value;
    }
    
    /// <summary>
    /// Gets the value of a dice group.  This may be affected by the
    /// <see cref="CalculationMethod"/> in use.  The dice are only rolled
    /// 'normally' if <see cref="CalculationMethod.Roll"/> is in use.
    /// </summary>
    /// <param name="sides">
    /// A <see cref="System.Int32"/>, the number of sides per die
    /// </param>
    /// <param name="dice">
    /// A <see cref="System.Int32"/>, the number of dice to be rolled
    /// </param>
    /// <param name="method">
    /// A <see cref="CalculationMethod"/>, the calculation method to use
    /// </param>
    /// <returns>
    /// A <see cref="System.Decimal"/>, the result
    /// </returns>
    private static decimal getDiceGroupValue(int sides,
                                             int dice,
                                             CalculationMethod method)
    {
      decimal output;
      
      switch(method)
      {
      case CalculationMethod.HighestDiceRolls:
        output = sides * dice;
        break;
      case CalculationMethod.Maximum:
        output = sides * dice;
        break;
      case CalculationMethod.LowestDiceRolls:
        output = dice;
        break;
      case CalculationMethod.Minimum:
        output = dice;
        break;
      case CalculationMethod.Mean:
        output = ((((decimal) sides - 1) / 2) + 1) * (decimal) dice;
        break;
      case CalculationMethod.Roll:
        output = randomiser.Next(1, sides) * dice;
        break;
      default:
        throw new ArgumentOutOfRangeException("method",
                                              "Invalid calculation method");
      }
      
      return output;
    }
    #endregion
    
    #region constructor
    static RollerCore()
    {
      randomiser = new Random();
    }
    #endregion
    
    #region containedClass
    public class DiceResult
    {
      public decimal Value;
      public DiceGroupOperator Operator;
      
      public DiceResult(decimal val, DiceGroupOperator op)
      {
        Value = val;
        Operator = op;
      }
    }
    #endregion
  }
}
