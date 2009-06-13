
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
    
    private const CalculationMethod
      DEFAULT_CALC_METHOD     = CalculationMethod.Roll;
    #endregion
    
    #region fields
    private List<DiceGroup> innerGroups;
    private int numberOfDice, sidesPerDie;
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
    
    public decimal GetValue()
    {
      return GetValue(DEFAULT_CALC_METHOD);
    }
    
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
    }
    #endregion
  }
}
