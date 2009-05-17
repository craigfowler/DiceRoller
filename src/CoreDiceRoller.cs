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
    
    internal decimal Minimum(DiceSpecification spec)
    {
      return getMinimum(spec);
    }
    
    internal decimal Minimum(string specString)
    {
      DiceSpecification spec;
      spec = DiceSpecification.Parse(specString);
      return getMinimum(spec);
    }
    
    internal decimal Maximum(DiceSpecification spec)
    {
      return getMaximum(spec);
    }
    
    internal decimal Maximum(string specString)
    {
      DiceSpecification spec;
      spec = DiceSpecification.Parse(specString);
      return getMaximum(spec);
    }
    
    internal decimal Mean(DiceSpecification spec)
    {
      return getMean(spec);
    }
    
    internal decimal Mean(string specString)
    {
      DiceSpecification spec;
      spec = DiceSpecification.Parse(specString);
      return getMean(spec);
    }
    
    private decimal[] rollDice(DiceSpecification spec)
    {
      spec.Options = options;
      return spec.Roll();
    }
    
    private decimal getMinimum(DiceSpecification spec)
    {
      spec.Options = options;
      return spec.MinimumResult;
    }
    
    private decimal getMaximum(DiceSpecification spec)
    {
      spec.Options = options;
      return spec.MaximumResult;
    }
    
    private decimal getMean(DiceSpecification spec)
    {
      spec.Options = options;
      return spec.MeanResult;
    }
    
#region constructor
    internal CoreDiceRoller()
    {
      options = new RollingOptions();
    }
#endregion
  }
}
