
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
    private int rerollStatsDiceLessThanOrEqualTo;
    private DnD3eRollingMethod rollMethod;
    #endregion
    
    #region properties
    
    public int DieRerollThreshold
    {
      get {
        return rerollStatsDiceLessThanOrEqualTo;
      }
      set {
        if(value >= 0 && value < 5)
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
      
      // TODO: Handle rerolling dice that come up less than
      // rerollStatsDiceLessThanOrEqualTo
      
      return output;
    }
    
    #endregion
    
    #region publicMethods
    
    public int[] RollStats()
    {
      int[] output;
      decimal[] tempOutput;
      
      if(statsSpec == null)
      {
        statsSpec = getDiceSpec(rollMethod);
      }
      
      tempOutput = statsSpec.Roll();
      output = new int[tempOutput.Length];
      for(int i = 0; i < tempOutput.Length; i++)
      {
        output[i] = (int) tempOutput[i];
      }
      
      return output;
    }
    
    #endregion
    
    #region constructor
    public DnD3e()
    {
      statsSpec = null;
      rerollStatsDiceLessThanOrEqualTo = 0;
      rollMethod = DnD3eRollingMethod.FourD6DropLowest;
    }
    #endregion
  }
}
