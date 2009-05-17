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
      set {
        if(value.HasValue && value > 0)
        {
          lowerBound = value;
        }
        else if(value == null)
        {
          lowerBound = null;
        }
      }
    }

    public Nullable<int> UpperBound {
      get {
        return upperBound;
      }
      set {
        if(value.HasValue && value > 0)
        {
          upperBound = value;
        }
        else if(value == null)
        {
          upperBound = null;
        }
      }
    }

    public Nullable<int> ExplodingThreshold {
      get {
        return explodingThreshold;
      }
      set {
        if(value.HasValue && value > 0)
        {
          explodingThreshold = value;
        }
        else if(value == null)
        {
          explodingThreshold = null;
        }
      }
    }

    public ExplodingStyle ExplodingStyle {
      get {
        return explodingStyle;
      }
      set {
        explodingStyle = value;
      }
    }

    public Nullable<int> ExplodingLimit {
      get {
        return explodingLimit;
      }
      set {
        if(value.HasValue && value > 0)
        {
          explodingLimit = value;
        }
        else if(value == null)
        {
          explodingLimit = null;
        }
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
