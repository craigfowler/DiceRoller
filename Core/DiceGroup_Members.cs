/*
 * DiceGroup Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace CraigFowler.Diceroller
{
  public partial class DiceGroup
  {
#region defaults
    private const DiceGroupDisplay DEFAULT_PARSING_STYLE =
      DiceGroupDisplay.Specification;
#endregion
    
    protected RollingOptions options;
    
    protected List<DiceGroup> innerGroups;
    protected GroupOperator? groupOperator;
    protected int? numDice, sidesPerDie, storedExplosions;
    protected decimal? storedResult;
    protected Random randomiser;
    
#region properties
    public List<DiceGroup> Groups {
      get {
        return innerGroups;
      }
      set {
        innerGroups = value;
      }
    }

    public GroupOperator Operator {
      get {
        GroupOperator output;
        
        if(groupOperator.HasValue)
        {
          output = groupOperator.Value;
        }
        else
        {
          throw new InvalidOperationException("Operator has not been set yet");
        }
        
        return output;
      }
      set {
        groupOperator = value;
      }
    }
    
    public bool OperatorIsMultiplication
    {
      get {
        bool output = false;
        
        if(groupOperator.HasValue &&
           (groupOperator.Value == GroupOperator.Multiply ||
            groupOperator.Value == GroupOperator.Divide))
        {
          output = true;
        }
        
        return output;
      }
    }
    
    public bool HasOperator
    {
      get {
        return groupOperator.HasValue;
      }
    }

    public Nullable<int> NumberOfDice {
      get {
        return numDice;
      }
      set {
        numDice = value;
      }
    }

    public Nullable<int> SidesPerDie {
      get {
        return sidesPerDie;
      }
      set {
        sidesPerDie = value;
      }
    }

    public RollingOptions Options {
      get {
        return options;
      }
      set {
        options = value;
      }
    }
    
    public decimal MinimumResult {
      get {
        return calculateMinimum();
      }
    }
    
    public decimal MaximumResult {
      get {
        return calculateMaximum();
      }
    }
    
    public decimal MeanResult {
      get {
        return calculateMean();
      }
    }
    
#endregion
    
#region methods
    public decimal GetValue(out int rollAgainExplosions)
    {
      rollAgainExplosions = 0;
      return calculateValue(ref rollAgainExplosions, false);
    }
    
    public decimal GetValue(out int rollAgainExplosions, bool reRoll)
    {
      rollAgainExplosions = 0;
      return calculateValue(ref rollAgainExplosions, reRoll);
    }
    
    public override string ToString()
    {
      return generateString(DEFAULT_PARSING_STYLE);
    }
    
    public string ToString(DiceGroupDisplay displayOptions)
    {
      return generateString(displayOptions);
    }
    
    protected virtual string generateString(DiceGroupDisplay options)
    {
      StringBuilder output = new StringBuilder();
      
      bool showResult =
        (options & DiceGroupDisplay.Results) == DiceGroupDisplay.Results;
      bool showSpec = 
        (options & DiceGroupDisplay.Specification) == DiceGroupDisplay.Specification;
      
      if(showSpec)
      {
        // Render the group's operator if it has one (not if a dice spec)
        if(groupOperator.HasValue)
        {
          switch(groupOperator.Value)
          {
          case GroupOperator.Add:
            output.Append("+");
            break;
          case GroupOperator.Divide:
            output.Append("/");
            break;
          case GroupOperator.Multiply:
            output.Append("*");
            break;
          case GroupOperator.Subtract:
            output.Append("-");
            break;
          }
        }
        
        // Render an opening bracket if we need one
        if(innerGroups.Count > 0)
        {
          output.Append("(");
          output.Append(renderNumeric(showResult));
          
          // Render all of the contained groups
          foreach(DiceGroup group in innerGroups)
          {
            output.Append(group.ToString(options));
          }
          
          output.Append(")");
        }
        else
        {
          output.Append(renderNumeric(showResult));
        }
      }
      
      return output.ToString();
    }
    
    protected string renderNumeric(bool showResult)
    {
      string output = String.Empty;
      
      if(numDice.HasValue && sidesPerDie.HasValue)
      {
        output = String.Format("{0}d{1}",
                               numDice.Value,
                               sidesPerDie.Value);
      }
      else if(numDice.HasValue)
      {
        output = numDice.Value.ToString();
      }
      
      if(showResult && output != String.Empty)
      {
        int explosions = 0;
        output += String.Format(":{0}", calculateValue(ref explosions));
      }
      
      return output;
    }
    
    protected decimal calculateValue(ref int explosions)
    {
      return calculateValue(ref explosions, false);
    }
    
    protected decimal calculateValue(ref int explosions,
                                     bool ignoreCachedResults)
    {
      decimal output;
      List<DiceResult> results;
      int groupExplosions;
      
#if DEBUG
      // Console.WriteLine("\nRolling Spec: '{0}'", this.ToString());
#endif
      
      /* As a safety feature, there needs to be a check that exploding dice
       * won't explode infinitely.
       * 
       * That means checking that the exploding threshold is more than the
       * lowest possible result of the group.
       * 
       * Always ignore exploding dice requests if no dice are being rolled (IE:
       * it's a static number, or the dice setting means that there is no range)
       */
      
      /* Remember to use the cached results (and number of explosions)
       * if they are available and we have not been told to ignore them
       */
      
      results = new List<DiceResult>();
      
      if(numDice.HasValue)
      {
        DiceResult newResult = new DiceResult();
        newResult.Value = calculateRoll();
        results.Add(newResult);
        // TODO:  Check for add-on explosions
      }
      
      for(int i = 0; i < innerGroups.Count; i++)
      {
        DiceResult newResult = new DiceResult();
        newResult.Value = innerGroups[i].GetValue(out groupExplosions,
                                                  ignoreCachedResults);
        if(i > 0)
        {
          newResult.Operator = innerGroups[i].Operator;
        }
        else if(i == 0 &&
                innerGroups[0].HasOperator &&
                innerGroups[0].Operator == GroupOperator.Add)
        {
          newResult.Value = 0 + newResult.Value;
        }
        else if(i == 0 &&
                innerGroups[0].HasOperator &&
                innerGroups[0].Operator == GroupOperator.Subtract)
        {
          newResult.Value = 0 - newResult.Value;
        }
        
        results.Add(newResult);
        explosions += groupExplosions;
      }
      
      for(int i = 1; i < results.Count; i++)
      {
        if(results[i].Operator == GroupOperator.Multiply)
        {
          results[i-1].Value = results[i-1].Value * results[i].Value;
          results.RemoveAt(i);
          i--;
          // TODO:  If we are rounding after every step, then round here!
        }
        else if(results[i].Operator == GroupOperator.Divide)
        {
          results[i-1].Value = results[i-1].Value / results[i].Value;
          results.RemoveAt(i);
          i--;
          // TODO:  If we are rounding after every step, then round here!
        }
      }
      
      for(int i = 1; i < results.Count; i++)
      {
        if(results[i].Operator == GroupOperator.Add)
        {
          results[i-1].Value = results[i-1].Value + results[i].Value;
          results.RemoveAt(i);
          i--;
          // TODO:  If we are rounding after every step, then round here!
        }
        else if(results[i].Operator == GroupOperator.Subtract)
        {
          results[i-1].Value = results[i-1].Value - results[i].Value;
          results.RemoveAt(i);
          i--;
          // TODO:  If we are rounding after every step, then round here!
        }
      }
      
      if(results.Count == 1)
      {
        output = results[0].Value;
      }
      else if(results.Count > 1)
      {
        string message = String.Format("Something is wrong here, I have {0} "+
                                       "results when I was expecting 1 or 0.",
                                       results.Count);
        throw new InvalidOperationException(message);
      }
      else
      {
        output = 0;
      }
      
      /* TODO: If the result of this dice group is equal to or above the
       * exploding threshold, then add an explosion on if we are using roll
       * again explosions
       */
      
      // TODO:  Write this next!
      
#if DEBUG
      // Console.WriteLine("Output for this group is {0}", output);
#endif
      return output;
    }
    
    protected int calculateRoll()
    {
      int lBound, uBound, output;
      
#if DEBUG
      /*
      Console.WriteLine("Has numDice: {0}, has sidesPerDie: {1}",
                        numDice.HasValue,
                        sidesPerDie.HasValue);
      Console.WriteLine("Options: {0}, SidesPerDie: {1}, NumDice: {2}",
                        options.ToString(),
                        sidesPerDie.ToString(),
                        numDice.ToString());
      */
#endif
      
      if(sidesPerDie.HasValue)
      {
        lBound = options.LowerBound.HasValue?
          options.LowerBound.Value : 1;
        uBound = options.UpperBound.HasValue?
          options.UpperBound.Value : sidesPerDie.Value + 1;
        
        output = 0;
        
        for(int i = 0; i < numDice.Value; i++)
        {
          output += randomiser.Next(lBound, uBound);
        }
      }
      else if(numDice.HasValue)
      {
        output = numDice.Value;
      }
      else
      {
        output = 0;
      }
      
      return output;
    }
    
    protected decimal calculateMinimum()
    {
      throw new NotImplementedException();
    }
    
    protected decimal calculateMaximum()
    {
      throw new NotImplementedException();
    }
    
    protected decimal calculateMean()
    {
      throw new NotImplementedException();
    }
#endregion
  }
}
