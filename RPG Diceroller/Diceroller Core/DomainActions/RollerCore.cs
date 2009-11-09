
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
    /// <exception cref="System.InvalidOperationException">
    /// <para>
    /// The downstream method <see cref="getResultsTotal(List<DiceResult>)"/>
    /// could throw this exception if there is an invalid operator in the
    /// results to process.
    /// </para>
    /// <para>
    /// This could also be thrown by the downstream method
    /// <see cref="getDiceGroupValue(DiceGroup,CalculationMethod)"/> if the
    /// settings of the dice group indicate that more dice results should be
    /// discarded than should be rolled.
    /// </para>
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The downstream method
    /// <see cref="getDiceGroupValue(DiceGroup,CalculationMethod)"/> could throw
    /// this exception if the parameter <paramref name="baseMethod"/> is not
    /// recognised.
    /// </exception>
    /// <exception cref="DivideByZeroException">
    /// The downstream method <see cref="getResultsTotal(List<DiceResult>)"/>
    /// could throw this exception if there is a division by zero in the
    /// totalling of the results.
    /// </exception>
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
        groupValue = getDiceGroupValue(group, method);
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
    /// <exception cref="System.InvalidOperationException">
    /// If for any reason the result count anything other than one single result
    /// after executing all of the applicable operators, then this means that
    /// one or more of the operators was invalid.
    /// </exception>
    /// <exception cref="DivideByZeroException">
    /// If the totalling of the dice results means that a division by zero
    /// occurs, then this exception will be thrown.
    /// </exception>
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
        throw new InvalidOperationException("Unrecognised operator");
      }
      
      return groupResults[0].Value;
    }
    
    /// <summary>
    /// Gets the value of a dice group.  This may be affected by the
    /// <see cref="CalculationMethod"/> in use.  The dice are only rolled
    /// 'normally' if <see cref="CalculationMethod.Roll"/> is in use.
    /// </summary>
    /// <param name="group">
    /// A <see cref="DiceGroup"/>, the dice group to roll
    /// </param>
    /// <param name="method">
    /// A <see cref="CalculationMethod"/>, the calculation method to use
    /// </param>
    /// <returns>
    /// A <see cref="System.Decimal"/>, the result
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// If the parameter <paramref name="method"/> is not a recognised
    /// calculation method then this exception will be thrown.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// If the number of dice rolls to discard is more than the number of dice
    /// being rolled, then this exception is thrown.
    /// </exception>
    private static decimal getDiceGroupValue(DiceGroup group,
                                             CalculationMethod method)
    {
      decimal output;
      int effectiveDice = group.Dice - (group.DiscardHighest + group.DiscardLowest);
      
      if(effectiveDice < 0)
      {
        throw new InvalidOperationException("Discarding more dice than are being rolled");
      }
      
      switch(method)
      {
      case CalculationMethod.HighestDiceRolls:
        output = group.Sides * effectiveDice;
        break;
      case CalculationMethod.Maximum:
        output = group.Sides * effectiveDice;
        break;
      case CalculationMethod.LowestDiceRolls:
        output = effectiveDice;
        break;
      case CalculationMethod.Minimum:
        output = effectiveDice;
        break;
      case CalculationMethod.Mean:
        output = ((((decimal) group.Sides - 1) / 2) + 1) * (decimal) effectiveDice;
        break;
      case CalculationMethod.Roll:
        output = rollDice(group.Sides,
                          group.Dice,
                          group.DiscardLowest,
                          group.DiscardHighest,
                          group.RerollLowerThan,
                          group.RerollHigherThan);
        break;
      default:
        throw new ArgumentOutOfRangeException("method", "Invalid calculation method");
      }
      
      return output;
    }
    
    private static int rollDice(int sides,
                                int dice,
                                int dropLowest,
                                int dropHighest,
                                int? rerollLow,
                                int? rerollHigh)
    {
      List<int> output = new List<int>();
      int result = 0, min, max;
      
      if(rerollLow.HasValue && rerollLow.Value < sides)
      {
        min = rerollLow.Value;
      }
      else
      {
        min = 1;
      }
      
      if(rerollHigh.HasValue && rerollHigh.Value < sides && rerollHigh.Value >= min)
      {
        max = rerollHigh.Value + 1;
      }
      else
      {
        max = sides + 1;
      }
      
      for(int i = 0; i < dice; i++)
      {
        output.Add(randomiser.Next(min, max));
      }
      
      output.Sort();
      output.RemoveRange(0, dropLowest);
      output.RemoveRange(output.Count - (dropHighest + 1), dropHighest);
      
      foreach(int roll in output)
      {
        result += roll;
      }
      
      return result;
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
