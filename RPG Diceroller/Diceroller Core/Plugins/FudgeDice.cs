
using System;
using CraigFowler.Gaming.Diceroller.DomainObjects;

namespace CraigFowler.Gaming.Diceroller.Plugins
{
  public class FudgeDice
  {
    private const string
      FUDGE_TEMPLATE                    = "{0}#1d3-2";
    
    #region fields
    
    private int[] results;
    private int numberOfDice;
    private DiceSpecification diceSpec;
    
    #endregion
    
    #region properties
    
    public int NumberOfDice
    {
      get {
        return numberOfDice;
      }
      set {
        if(value > 0)
        {
          numberOfDice = value;
          diceSpec.SpecificationString = String.Format(FUDGE_TEMPLATE,
                                                       numberOfDice);
        }
        else
        {
          numberOfDice = 0;
        }
      }
    }
    
    public int Negative
    {
      get {
        return getCount(Results, -1);
      }
    }
    
    public int Neutral
    {
      get {
        return getCount(Results, 0);
      }
    }
    
    public int Positive
    {
      get {
        return getCount(Results, 1);
      }
    }
    
    public int Overall
    {
      get {
        int output = 0;
        
        foreach(int roll in Results)
        {
          output += roll;
        }
        
        return output;
      }
    }
    
    public int[] Results
    {
      get {
        if(results == null)
        {
          results = rollDice();
        }
        
        return results;
      }
    }
    
    public string ResultText
    {
      get {
        System.Text.StringBuilder output = new System.Text.StringBuilder();
        
        foreach(int roll in Results)
        {
          output.Append(formatFudgeDie(roll));
        }
        
        return output.ToString();
      }
    }
    
    #endregion
    
    #region publicMethods
    
    public void Roll()
    {
      results = rollDice();
    }
    
    #endregion
    
    #region privateMethods
    
    private int getCount(int[] input, int countedValue)
    {
      int output = 0;
      
      foreach(int roll in input)
      {
        if(roll == countedValue)
        {
          output ++;
        }
      }
      
      return output;
    }
    
    private string formatFudgeDie(int result)
    {
      string output;
      
      switch(result)
      {
      case -1:
        output = "-";
        break;
      case 0:
        output = " ";
        break;
      case 1:
        output = "+";
        break;
      default:
        throw new ArgumentOutOfRangeException("result", "Unsupported result");
      }
      
      return String.Format("[{0}]", output);
    }
    
    private int[] rollDice()
    {
      return convertDecimalArray(diceSpec.Roll(CalculationMethod.Roll));
    }
    
    private int[] convertDecimalArray(decimal[] input)
    {
      int[] output = new int[input.Length];
      
      for(int i = 0; i < input.Length; i++)
      {
        output[i] = (int) input[i];
      }
      
      return output;
    }
    
    #endregion
    
    #region constructors
    
    public FudgeDice()
    {
      results = null;
      numberOfDice = 0;
      diceSpec = new DiceSpecification();
    }
    
    public FudgeDice(int dice) : this()
    {
      NumberOfDice = dice;
    }
    
    #endregion
  }
}
