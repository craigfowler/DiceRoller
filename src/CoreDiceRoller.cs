/*
 * CoreDiceRoller Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Diceroller
{
  internal sealed class CoreDiceRoller
  {
    private RollingOptions options;
    
    public RollingOptions Options {
      get {
        return options;
      }
    }
    
    internal CoreDiceRoller()
    {
      options = new RollingOptions();
    }
  }
}
