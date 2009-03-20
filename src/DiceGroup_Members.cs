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
    protected RollingOptions options;
    
    protected List<DiceGroup> innerGroups;
    protected GroupOperator? groupOperator;
    protected int? numDice;
    protected int? sidesPerDie;
    
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
    
    internal decimal GetValue(out int rollAgainExplosions)
    {
      rollAgainExplosions = 0;
      return calculateValue(ref rollAgainExplosions);
    }
    
    protected decimal calculateValue(ref int explosions)
    {
      throw new NotImplementedException();
    }
  }
}
