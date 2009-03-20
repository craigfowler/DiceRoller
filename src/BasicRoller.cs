/*
 * BasicRoller Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Diceroller
{
  public class BasicRoller
  {
    protected static RoundingOptions defaultRounding;
    
    public virtual RoundingMethod Rounding {
      get {
        return defaultRounding.Rounding;
      }
      set {
        defaultRounding.Rounding = value;
      }
    }
    
    public virtual int RoundingDigits {
      get {
        return defaultRounding.RoundingDigits;
      }
      set {
        defaultRounding.RoundingDigits = value;
      }
    }
    
    public virtual bool RoundEveryStep {
      get {
        return defaultRounding.RoundEveryStep;
      }
      set {
        defaultRounding.RoundEveryStep = value;
      }
    }
    
    public BasicRoller()
    {
      defaultRounding = new RoundingOptions();
    }
    
    public virtual int[] Roll(string diceSpec)
    {
      throw new NotImplementedException();
    }
  }
}
