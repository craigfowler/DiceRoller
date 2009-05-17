/*
 * RoundingOptions Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Diceroller
{
  public class RoundingOptions
  {
#region defaults
    public const RoundingMethod DEFAULT_ROUNDING =
      RoundingMethod.MidpointAwayFromZero;
    public const int DEFAULT_DECIMAL_PLACES = 0;
    public const bool DEFUALT_ROUND_EVERY_STEP = false;
#endregion
    
    public RoundingMethod RoundingType;
    protected int decimalPlaces;
    public bool RoundEveryStep;
    
    public int RoundingDigits {
      get {
        return decimalPlaces;
      }
      set {
        if(value >= 0)
        {
          decimalPlaces = value;
        }
        else
        {
          decimalPlaces = 0;
        }
      }
    }
    
    public RoundingOptions()
    {
      RoundingType = DEFAULT_ROUNDING;
      RoundingDigits = DEFAULT_DECIMAL_PLACES;
      RoundEveryStep = DEFUALT_ROUND_EVERY_STEP;
    }
  }
}
