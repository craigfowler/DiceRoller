/*
 * DiceGroup Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using System.Collections.Generic;

namespace CraigFowler.Diceroller
{
  internal partial class DiceGroup
  {
#region defaults
    private const DiceGroupDisplay DEFAULT_PARSING_STYLE =
      DiceGroupDisplay.ParsedSpecification;
#endregion
    
    protected RollingOptions options;
    
    protected List<DiceGroup> innerGroups;
    protected GroupOperator? groupOperator;
    protected int? numDice;
    protected int? sidesPerDie;
    
#region properties
    internal List<DiceGroup> Groups {
      get {
        return innerGroups;
      }
      set {
        innerGroups = value;
      }
    }

    internal GroupOperator Operator {
      get {
        GroupOperator output;
        
        if(groupOperator.HasValue)
        {
          output = groupOperator.Value;
        }
        else
        {
          throw new InvalidOperationException("Operator has not been set yet");
        }
        
        return output;
      }
      set {
        groupOperator = value;
      }
    }

    internal Nullable<int> NumberOfDice {
      get {
        return numDice;
      }
      set {
        numDice = value;
      }
    }

    internal Nullable<int> SidesPerDie {
      get {
        return sidesPerDie;
      }
      set {
        sidesPerDie = value;
      }
    }

    internal RollingOptions Options {
      get {
        return options;
      }
      set {
        options = value;
      }
    }
    
    internal decimal MinimumResult {
      get {
        return calculateMinimum();
      }
    }
    
    internal decimal MaximumResult {
      get {
        return calculateMaximum();
      }
    }
    
    internal decimal MeanResult {
      get {
        return calculateMean();
      }
    }
    
#endregion
    
#region methods
    internal decimal GetValue(out int rollAgainExplosions)
    {
      rollAgainExplosions = 0;
      return calculateValue(ref rollAgainExplosions);
    }
    
    public override string ToString()
    {
      return generateString(DEFAULT_PARSING_STYLE);
    }
    
    public string ToString(DiceGroupDisplay displayOptions)
    {
      return generateString(displayOptions);
    }
    
    protected virtual string generateString(DiceGroupDisplay options)
    {
      throw new NotImplementedException();
    }
    
    protected decimal calculateValue(ref int explosions)
    {
      /* As a safety feature, there needs to be a check that exploding dice
       * won't explode infinitely.
       * 
       * That means checking that the exploding threshold is more than the
       * lowest possible result of the group.
       * 
       * Always ignore exploding dice requests if no dice are being rolled (IE:
       * it's a static number, or the dice setting means that there is no range)
       */
      throw new NotImplementedException();
    }
    
    protected decimal calculateMinimum()
    {
      throw new NotImplementedException();
    }
    
    protected decimal calculateMaximum()
    {
      throw new NotImplementedException();
    }
    
    protected decimal calculateMean()
    {
      throw new NotImplementedException();
    }
#endregion
  }
}
