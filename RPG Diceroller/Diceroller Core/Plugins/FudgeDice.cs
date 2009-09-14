
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
        if(results == null)
        {
          results = GetResults();
        }
        return getCount(results, -1);
      }
    }
    
    public int Neutral
    {
      get {
        if(results == null)
        {
          results = GetResults();
        }
        return getCount(results, 0);
      }
    }
    
    public int Positive
    {
      get {
        if(results == null)
        {
          results = GetResults();
        }
        return getCount(results, 1);
      }
    }
    
    public int Overall
    {
      get {
        int output = 0;
        
        Roll();
        
        foreach(int roll in results)
        {
          output += roll;
        }
        
        return output;
      }
    }
    
    public string ResultText
    {
      get {
        System.Text.StringBuilder output = new System.Text.StringBuilder();
        
        Roll();
        
        foreach(int roll in results)
        {
          output.Append(formatFudgeDie(roll));
        }
        
        return output.ToString();
      }
    }
    #endregion
    
    #region publicMethods
    public int[] GetResults()
    {
      int[] output;
      decimal[] tempOutput;
      
      tempOutput = diceSpec.Roll(CalculationMethod.Roll);
      output = new int[tempOutput.Length];
      
      for(int i = 0; i < tempOutput.Length; i++)
      {
        output[i] = (int) tempOutput[i];
      }
      
      return output;
    }
    
    public void Roll()
    {
      results = GetResults();
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
