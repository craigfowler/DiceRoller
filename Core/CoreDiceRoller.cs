/*
 * CoreDiceRoller Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Diceroller
{
  public sealed class CoreDiceRoller
  {
    private RollingOptions options;
    
    public RollingOptions Options {
      get {
        return options;
      }
    }
    
    public decimal[] Roll(DiceSpecification spec)
    {
      return rollDice(spec);
    }
    
    public decimal[] Roll(string specString)
    {
      DiceSpecification spec;
      spec = DiceSpecification.Parse(specString);
      return rollDice(spec);
    }
    
    public decimal Minimum(DiceSpecification spec)
    {
      return getMinimum(spec);
    }
    
    public decimal Minimum(string specString)
    {
      DiceSpecification spec;
      spec = DiceSpecification.Parse(specString);
      return getMinimum(spec);
    }
    
    public decimal Maximum(DiceSpecification spec)
    {
      return getMaximum(spec);
    }
    
    public decimal Maximum(string specString)
    {
      DiceSpecification spec;
      spec = DiceSpecification.Parse(specString);
      return getMaximum(spec);
    }
    
    public decimal Mean(DiceSpecification spec)
    {
      return getMean(spec);
    }
    
    public decimal Mean(string specString)
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
    public CoreDiceRoller()
    {
      options = new RollingOptions();
    }
#endregion
  }
}
