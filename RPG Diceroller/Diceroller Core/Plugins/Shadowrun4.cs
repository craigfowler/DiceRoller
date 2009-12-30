
using System;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Gaming.Diceroller.Plugins
{
  public class Shadowrun4
  {
    #region fields
    
    private int dicePool, edgeUsed, gremlins;
    protected DiceSpecification Specification; 
    
    #endregion

    #region properties
    
    public int DicePool
    {
      get {
        return dicePool;
      }
      set {
        if(value >= 0)
        {
          dicePool = value;
          Specification = null;
        }
      }
    }

    public int EdgeUsed
    {
      get {
        return edgeUsed;
      }
      set {
        if(value >= 0)
        {
          edgeUsed = value;
          Specification = null;
        }
      }
    }

    public int Gremlins
    {
      get {
        return gremlins;
      }
      set {
        if(value >= 0 && value <= 4)
        {
          gremlins = value;
          Specification = null;
        }
      }
    }
    
    protected bool RuleOfSix
    {
      get {
        return EdgeUsed > 0;
      }
    }
    
    protected int TotalDice
    {
      get {
        return DicePool + EdgeUsed;
      }
    }
    
    #endregion
    
    #region methods
    
    public int Successes(out Shadowrun4Glitch glitch)
    {
      decimal[] results;
      int successes, ones, glitchThreshold;
      
      if(Specification == null)
      {
        Specification = new DiceSpecification("d6");
        Specification.NumberOfRolls = TotalDice;
        if(RuleOfSix)
        {
          Specification.RollAgainThreshold = 6;
        }
        else
        {
          Specification.RollAgainThreshold = null;
        }
      }
      
      results = Specification.Roll();
      glitchThreshold = (((results.Length / 2) - Gremlins) < 0)? 0 : ((results.Length / 2) - Gremlins);
      countResults(results, out successes, out ones);
      
      if(ones >= glitchThreshold)
      {
        glitch = (successes == 0)? Shadowrun4Glitch.Critical : Shadowrun4Glitch.Normal;
      }
      else
      {
        glitch = Shadowrun4Glitch.None;
      }
      
      return successes;
    }
    
    public override string ToString ()
    {
      int successes;
      Shadowrun4Glitch glitch;
      string output;
      
      successes = Successes(out glitch);
      
      if(glitch == Shadowrun4Glitch.None)
      {
        output = String.Format("{0} successes", successes);
      }
      else
      {
        output = String.Format("{0} successes, {1}!",
                               successes,
                               glitch == Shadowrun4Glitch.Normal? "glitch" : "critical glitch");
      }
      
      return output;
    }
    
    private void countResults(decimal[] results, out int successes, out int ones)
    {
      successes = 0;
      ones = 0;
      
      foreach(decimal result in results)
      {
        if(result >= 5m)
        {
          successes ++;
        }
        else if(result == 1m)
        {
          ones ++;
        }
      }
      
      return;
    }
    
    #endregion
    
    #region constructor
    
    public Shadowrun4()
    {
      DicePool = 0;
      EdgeUsed = 0;
      Gremlins = 0;
      Specification = null;
    }
    
    public Shadowrun4(int dice): this()
    {
      DicePool = dice;
    }
    
    #endregion
  }
}
