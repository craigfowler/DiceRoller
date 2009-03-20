/*
 * RollingOptions Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;

namespace CraigFowler.Diceroller
{
  public sealed class RollingOptions
  {
#region defaults
    public const ExplodingStyle DEFAULT_EXPLODING_STYLE = ExplodingStyle.None;
#endregion
    
    private RoundingOptions rounding;
    private int? lowerBound;
    private int? upperBound;
    private int? explodingThreshold;
    private ExplodingStyle explodingStyle;
    private int? explodingLimit;
    
    public Nullable<int> LowerBound {
      get {
        return lowerBound;
      }
    }

    public Nullable<int> UpperBound {
      get {
        return upperBound;
      }
    }

    public Nullable<int> ExplodingThreshold {
      get {
        return explodingThreshold;
      }
    }

    public ExplodingStyle ExplodingStyle {
      get {
        return explodingStyle;
      }
    }

    public Nullable<int> ExplodingLimit {
      get {
        return explodingLimit;
      }
    }

    public RoundingOptions Rounding {
      get {
        return rounding;
      }
    }
    
    public RollingOptions()
    {
      rounding = new RoundingOptions();
      lowerBound = null;
      upperBound = null;
      explodingThreshold = null;
      explodingStyle = DEFAULT_EXPLODING_STYLE;
      explodingLimit = null;
    }
  }
}
