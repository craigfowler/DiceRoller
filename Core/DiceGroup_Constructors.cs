/*
 * DiceGroup_Constructors Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using System.Collections.Generic;

namespace CraigFowler.Diceroller
{
  public partial class DiceGroup
  {
    internal DiceGroup()
    {
      initProtectedVars();
    }
    
    public DiceGroup(int number)
    {
      initProtectedVars();
      if(number > 0)
      {
        numDice = number;
      }
      else
      {
        numDice = 0;
      }
    }
    
    public DiceGroup(int howMany, int sides)
    {
      initProtectedVars();
      if(howMany > 0)
      {
        numDice = howMany;
      }
      else
      {
        numDice = 0;
      }
      if(sides > 0)
      {
        sidesPerDie = sides;
      }
      else
      {
        sidesPerDie = 0;
      }
    }
    
    public DiceGroup(RollingOptions opts)
    {
      initProtectedVars();
      options = opts;
    }
    
    public DiceGroup(int number, RollingOptions opts)
    {
      initProtectedVars();
      if(number > 0)
      {
        numDice = number;
      }
      else
      {
        numDice = 0;
      }
      options = opts;
    }
    
    public DiceGroup(int howMany, int sides, RollingOptions opts)
    {
      initProtectedVars();
      if(howMany > 0)
      {
        numDice = howMany;
      }
      else
      {
        numDice = 0;
      }
      if(sides > 0)
      {
        sidesPerDie = sides;
      }
      else
      {
        sidesPerDie = 0;
      }
      options = opts;
    }
    
    public DiceGroup(GroupOperator oper, int number)
    {
      initProtectedVars();
      if(number > 0)
      {
        numDice = number;
      }
      else
      {
        numDice = 0;
      }
      groupOperator = oper;
    }
    
    public DiceGroup(GroupOperator oper, int howMany, int sides)
    {
      initProtectedVars();
      if(howMany > 0)
      {
        numDice = howMany;
      }
      else
      {
        numDice = 0;
      }
      if(sides > 0)
      {
        sidesPerDie = sides;
      }
      else
      {
        sidesPerDie = 0;
      }
      groupOperator = oper;
    }
    
    public DiceGroup(GroupOperator oper, RollingOptions opts)
    {
      initProtectedVars();
      options = opts;
      groupOperator = oper;
    }
    
    public DiceGroup(GroupOperator oper, int number, RollingOptions opts)
    {
      initProtectedVars();
      if(number > 0)
      {
        numDice = number;
      }
      else
      {
        numDice = 0;
      }
      options = opts;
      groupOperator = oper;
    }
    
    public DiceGroup(GroupOperator oper,
                       int howMany,
                       int sides,
                       RollingOptions opts)
    {
      initProtectedVars();
      if(howMany > 0)
      {
        numDice = howMany;
      }
      else
      {
        numDice = 0;
      }
      if(sides > 0)
      {
        sidesPerDie = sides;
      }
      else
      {
        sidesPerDie = 0;
      }
      options = opts;
      groupOperator = oper;
    }
    
    protected void initProtectedVars()
    {
      innerGroups = new List<DiceGroup>();
      groupOperator = null;
      numDice = null;
      sidesPerDie = null;
      options = new RollingOptions();
      storedExplosions = null;
      storedResult = null;
      randomiser = new Random();
    }
    
#region containedClasses
    protected class DiceResult
    {
      public decimal Value;
      public GroupOperator Operator;
    }
#endregion
  }
}
