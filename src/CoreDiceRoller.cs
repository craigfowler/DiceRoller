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
    
    internal RollingOptions Options {
      get {
        return options;
      }
    }
    
    internal decimal[] Roll(DiceSpecification spec)
    {
      return rollDice(spec);
    }
    
    internal decimal[] Roll(string specString)
    {
      DiceSpecification spec;
      spec = DiceSpecification.Parse(specString);
      return rollDice(spec);
    }
    
    private decimal[] rollDice(DiceSpecification spec)
    {
      return spec.Roll();
    }
    
#region constructor
    internal CoreDiceRoller()
    {
      options = new RollingOptions();
    }
#endregion
  }
}
