
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace CraigFowler.Gaming.Diceroller.DomainObjects
{
  public class DiceGroup
  {
    #region constants
    
    private const string
      INVALID_NUM_DICE_ERROR  = "Number of dice must be more than zero.",
      INVALID_NUM_SIDES_ERROR = "Number of sides must be more than zero.";
    
    private const string
      ADDITION_SYMBOL         = "+",
      SUBTRACTION_SYMBOL      = "-",
      MULTIPLICATION_SYMBOL   = "*",
      DIVISION_SYMBOL         = "/",
      DICEGROUP_OPEN_SYMBOL   = "(",
      DICEGROUP_CLOSE_SYMBOL  = ")";
    
    #endregion
    
    #region fields
    private List<DiceGroup> innerGroups;
    private int numberOfDice, sidesPerDie, discardLowestDice, discardHighestDice;
    private int? rerollLowerThan, rerollHigherThan;
    private DiceGroupOperator groupOperator;
    #endregion
    
    #region properties
	
    public DiceGroupOperator Operator
    {
      get {
        return groupOperator;
      }
      set {
        groupOperator = value;
      }
    }

    public List<DiceGroup> Groups
    {
      get {
        return innerGroups;
      }
      set {
        innerGroups = value;
      }
    }

    public int Dice
    {
      get {
        return numberOfDice;
      }
      set {
        if(value > 0)
        {
          numberOfDice = value;
        }
        else
        {
          throw new InvalidOperationException(INVALID_NUM_DICE_ERROR);
        }
      }
    }

    public int Sides
    {
      get {
        return sidesPerDie;
      }
      set {
        if(value > 0)
        {
          sidesPerDie = value;
        }
        else
        {
          throw new InvalidOperationException(INVALID_NUM_DICE_ERROR);
        }
      }
    }
    
    public int DiscardHighest
    {
      get {
        return discardHighestDice;
      }
      set {
        if(value >= 0)
        {
          discardHighestDice = value;
        }
      }
    }
    
    public int DiscardLowest
    {
      get {
        return discardLowestDice;
      }
      set {
        if(value >= 0)
        {
          discardLowestDice = value;
        }
      }
    }
    
    public int? RerollLowerThan
    {
      get {
        return rerollLowerThan;
      }
      set {
        if(!value.HasValue || value.Value > 1)
        {
          rerollLowerThan = value;
        }
      }
    }
    
    
    public int? RerollHigherThan
    {
      get {
        return rerollHigherThan;
      }
      set {
        if(!value.HasValue || value.Value > 1)
        {
          rerollHigherThan = value;
        }
      }
    }
    
    #endregion
    
    #region privateMethods
    
    private string getNumericString()
    {
      string output;
      
      if(this.Dice == 1 && this.Sides > 1)
      {
        output = String.Format("d{1}", this.Dice, this.Sides);
      }
      if(this.Sides > 1)
      {
        output = String.Format("{0}d{1}", this.Dice, this.Sides);
      }
      else if(this.Dice > 0)
      {
        output = this.Dice.ToString();
      }
      else
      {
        output = String.Empty;
      }
      
      return output;
    }
    
    private string getOperatorString(bool first)
    {
      string output;
      
      if(first && this.Operator == DiceGroupOperator.Add)
      {
        output = String.Empty;
      }
      else
      {
        switch(this.Operator)
        {
        case DiceGroupOperator.Add:
          output = ADDITION_SYMBOL;
          break;
        case DiceGroupOperator.Subtract:
          output = SUBTRACTION_SYMBOL;
          break;
        case DiceGroupOperator.Multiply:
          output = MULTIPLICATION_SYMBOL;
          break;
        case DiceGroupOperator.Divide:
          output = DIVISION_SYMBOL;
          break;
        default:
          output = String.Empty;
          break;
        }
      }
      
      return output;
    }
    
    #endregion
    
    #region publicMethods
    
    public override string ToString()
    {
      return ToString(true, false);
    }
    
    public string ToString(bool topLevel, bool first)
    {
      StringBuilder output = new StringBuilder();
      bool innerFirst;
      
      output.Append(getOperatorString(first));
      
      if(this.Groups.Count > 0)
      {
        if(!topLevel)
        {
          output.Append(DICEGROUP_OPEN_SYMBOL);
        }
        
        innerFirst = true;
        foreach(DiceGroup group in this.Groups)
        {
          output.Append(group.ToString(false, innerFirst));
          innerFirst = false;
        }
        
        if(!topLevel)
        {
          output.Append(DICEGROUP_CLOSE_SYMBOL);
        }
      }
      else
      {
        output.Append(getNumericString());
      }
      
      return output.ToString();
    }
    
    /// <summary>
    /// Gets the value of the dice group by performing a normal 'roll' of the
    /// dice.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Decimal"/> - the result.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// The downstream method
    /// <see cref="DomainActions.RollerCore.CalculateValue(DiceGroup,CalculationMethod)"/>
    /// could throw this exception if there is an invalid operator in the
    /// results to process.
    /// </exception>
    /// <exception cref="DivideByZeroException">
    /// The downstream method
    /// <see cref="DomainActions.RollerCore.CalculateValue(DiceGroup,CalculationMethod)"/>
    /// could throw this exception if there is a division by zero in calculating
    /// the results.
    /// </exception>
    public decimal GetValue()
    {
      return GetValue(DiceSpecification.DefaultCalculationMethod);
    }
    
    /// <summary>
    /// <para>
    /// Gets the value of the dice group.  Depending on the
    /// <paramref name="method"/> used this could actually roll the dice, or it
    /// could instead return the average (mean) roll, minimum roll etc.
    /// </para>
    /// <para>
    /// Internally, this simply serves as a wrapper around the method:
    /// <see cref="DomainActions.RollerCore.CalculateValue(DiceGroup,CalculationMethod)"/>,
    /// thus any exceptions encountered will have been thrown from there.
    /// </para>
    /// </summary>
    /// <param name="method">
    /// A <see cref="CalculationMethod"/>, the calculation method to use in
    /// calculating the results.
    /// </param>
    /// <returns>
    /// A <see cref="System.Decimal"/> - the result.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown if there is an invalid operator in the dice group.  It is also
    /// thrown if the combination of <see cref="DiscardHighest"/> and
    /// <see cref="DiscardLowest"/> result in more dice being discarded than are
    /// rolled.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// This is thrown is the parameter <paramref name="method"/> is not
    /// recognised s a valid calculation method.
    /// </exception>
    /// <exception cref="DivideByZeroException">
    /// This is thrown if calculating the value of this dice group results in a
    /// division by zero.
    /// </exception>
    public decimal GetValue(CalculationMethod method)
    {
      return DomainActions.RollerCore.CalculateValue(this, method);
    }
    
    #endregion
    
    #region constructor
    
    public DiceGroup()
    {
      innerGroups = new List<DiceGroup>();
      numberOfDice = 1;
      sidesPerDie = 1;
      groupOperator = 0;
      discardHighestDice = 0;
      discardLowestDice = 0;
      rerollHigherThan = null;
      rerollLowerThan = null;
    }
    
    #endregion
  }
}
