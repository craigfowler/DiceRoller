
using System;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Gaming.Diceroller.Plugins
{
  public class DnD3e
  {
    #region constants
    private const string
      STATS_TEMPLATE_4D6      = "6#4d6",
      STATS_TEMPLATE_3D6      = "6#3d6";
    #endregion
    
    #region fields
    private DiceSpecification statsSpec;
    private int? rerollStatsDiceLessThanOrEqualTo;
    private DnD3eRollingMethod rollMethod;
    #endregion
    
    #region properties
    
    /// <value>
    /// Rerolls any individual die that comes up less than this number.
    /// Must be between one and five, or null. Default is null (no rerolling).
    /// </value>
    public int? DieRerollThreshold
    {
      get {
        return rerollStatsDiceLessThanOrEqualTo;
      }
      set {
        if(!value.HasValue || (value.Value > 0 && value.Value < 6))
        {
          rerollStatsDiceLessThanOrEqualTo = value;
          statsSpec = null;
        }
      }
    }
    
    public bool IsEligibleForReroll
    {
      get {
        throw new NotImplementedException();
      }
    }
    
    /// <value>
    /// Gets or sets the D&D 3.x rolling method for this stat block, default is "4d6 drop lowest".
    /// </value>
    public DnD3eRollingMethod RollingMethod
    {
      get {
        return rollMethod;
      }
      set {
        rollMethod = value;
        statsSpec = null;
      }
    }
    
    #endregion
    
    #region privateMethods
    
    private DiceSpecification getDiceSpec(DnD3eRollingMethod method)
    {
      DiceSpecification output;
      
      switch(method)
      {
      case DnD3eRollingMethod.FourD6DropLowest:
        output = new DiceSpecification(STATS_TEMPLATE_4D6);
        output.GetDice().DiscardLowest = 1;
        break;
      case DnD3eRollingMethod.ThreeD6:
        output = new DiceSpecification(STATS_TEMPLATE_3D6);
        break;
      default:
        throw new ArgumentOutOfRangeException("method",
                                              "Unrecognised rolling method");
      }
      
      output.GetDice().RerollLowerThan = DieRerollThreshold;
      
      return output;
    }
    
    private int[] convertDecimalArray(decimal[] inputArray)
    {
      int[] output;
      
      output = new int[inputArray.Length];
      for(int i = 0; i < inputArray.Length; i++)
      {
        output[i] = (int) inputArray[i];
      }
      
      return output;
    }
    
    #endregion
    
    #region publicMethods
    
    /// <summary>
    /// Rolls a set of D&D 3.x stats using the specified parameters.
    /// </summary>
    /// <returns>
    /// A <see cref="System.Int32"/> array containing six results.
    /// </returns>
    public int[] RollStats()
    {
      if(statsSpec == null)
      {
        statsSpec = getDiceSpec(rollMethod);
      }
      
      return convertDecimalArray(statsSpec.Roll());
    }
    
    public override string ToString ()
    {
      int[] results = RollStats();
      
      return string.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                           results[0],
                           results[1],
                           results[2],
                           results[3],
                           results[4],
                           results[5]);
    }

    
    #endregion
    
    #region constructor
    
    public DnD3e()
    {
      statsSpec = null;
      rerollStatsDiceLessThanOrEqualTo = null;
      rollMethod = DnD3eRollingMethod.FourD6DropLowest;
    }
    
    #endregion
  }
}
