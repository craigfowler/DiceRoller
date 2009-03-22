/*
 * DiceSpecification Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Diceroller
{
  internal sealed class DiceSpecification : DiceGroup
  {
    private int numberOfRolls;
    
    internal int NumberOfRolls {
      get {
        return numberOfRolls;
      }
      set {
        if(value >= 0)
        {
          numberOfRolls = value;
        }
        else
        {
          numberOfRolls = 0;
        }
      }
    }
    
    protected override string generateString(DiceGroupDisplay options)
    {
      throw new NotImplementedException();
    }
    
#region constructors
    internal DiceSpecification() : base()
    {
      numberOfRolls = 1;
    }
#endregion
    
#region staticMembers
    internal static DiceSpecification Parse(string diceSpec)
    {
      return parseDiceSpec(diceSpec);
    }
    
    private static DiceSpecification parseDiceSpec(string diceSpec)
    {
      throw new NotImplementedException();
    }
#endregion
  }
}
