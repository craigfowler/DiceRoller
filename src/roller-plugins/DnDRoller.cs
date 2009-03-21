/*
 * SpecialistRoller Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using CraigFowler.Diceroller;

namespace CraigFowler.Diceroller.Plugins
{
  public class DnDRoller : BasicRoller
  {
    public DnDRoller() : base()
    { }
    
    public new decimal[] Roll (string diceSpec)
    {
      throw new InvalidOperationException("DnDRoller has no Roll(string) " +
                                          "method");
    }
    
    public int[] RollStats(DnDStatsRollingMethod method)
    {
      throw new NotImplementedException();
    }
  }
}
