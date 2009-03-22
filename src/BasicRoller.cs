/*
 * BasicRoller Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Diceroller
{
  public class BasicRoller
  {
#region settings
    protected static RoundingOptions defaultRounding;
    
    public virtual RoundingMethod Rounding {
      get {
        return defaultRounding.RoundingType;
      }
      set {
        defaultRounding.RoundingType = value;
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
#endregion
    
#region constructors
    public BasicRoller()
    {
      defaultRounding = new RoundingOptions();
    }
#endregion
    
    public virtual decimal[] Roll(string diceSpec)
    {
      throw new NotImplementedException();
    }
  }
}
