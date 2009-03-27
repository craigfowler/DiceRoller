/*
 * DiceSpecification Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CraigFowler.Diceroller
{
  internal sealed class DiceSpecification : DiceGroup
  {
    /* A parse-able thing is:
     * Leading operator (optional)
     * Open Bracket (optional)
     * Some dice or a number (optional)
     * Close bracket (optional)
     */
    
#region constants
    private const string
      MATCH_WHITESPACE = @"\s",
      MATCH_NUMBER_OF_ROLLS = @"^(\d+)#",
      MATCH_DICE_GROUP = @"([-+*x/]*)(\()?(((\d*)[Dd]([%\d]+))|(\d+))?(\))?";
#endregion
    private int numberOfRolls;
    
    internal int NumberOfRolls {
      get {
        return numberOfRolls;
      }
      set {
        if(value >= 0)
        {
          numberOfRolls = value;
        }
        else
        {
          numberOfRolls = 0;
        }
      }
    }
    
    protected override string generateString (DiceGroupDisplay options)
    {
      StringBuilder output = new StringBuilder();
      
      if((options & DiceGroupDisplay.Specification) ==
         DiceGroupDisplay.Specification)
      {
        if(numberOfRolls > 1)
        {
          output.Append(String.Format("{0}#", numberOfRolls.ToString()));
        }
        
        if(numberOfRolls > 0)
        {
          foreach(DiceGroup group in innerGroups)
          {
            output.Append(group.ToString(options));
          }
        }
      }
      
      return output.ToString();
    }
    
    internal decimal[] Roll()
    {
      List<decimal> output = new List<decimal>();
      int rollsPending = numberOfRolls, rollAgainExplosions;
      
      while(rollsPending > 0)
      {
        output.Add(this.GetValue(out rollAgainExplosions));
        rollsPending --;
        rollsPending += rollAgainExplosions;
      }
      
      return output.ToArray();
    }
    
#region constructors
    internal DiceSpecification() : base()
    {
      numberOfRolls = 1;
    }
#endregion
    
#region staticMembers
    internal static DiceSpecification Parse(string diceSpec)
    {
      DiceSpecification output;
      Queue<Match> groupMatches;
      Regex matchDiceGroups = new Regex(MATCH_DICE_GROUP);
      
      /* Parse the basics of the dice spec (strip whitespace and get the number
       * of rolls
       */
      output = parseSpecBasics(ref diceSpec);
      
#if DEBUG
      Console.WriteLine("\n{1} rolls, spec: '{0}'",
                        diceSpec,
                        output.NumberOfRolls);
#endif
      
      /* Now, the dice specification is ready for parsing groups.
       * Use a regex to match the dice groups in the specification, then put
       * them into a queue of regex matches.
       */
      groupMatches = new Queue<Match>();
      foreach(Match group in matchDiceGroups.Matches(diceSpec))
      {
        if(group.Value != String.Empty)
        {
          groupMatches.Enqueue(group);
        }
      }
      
      output = (DiceSpecification) parseGroupMatches(groupMatches,
                                                     (DiceGroup) output);
      
      return output;
    }
    
    private static DiceGroup parseGroupMatches(Queue<Match> matches,
                                               DiceGroup input)
    {
      bool enter, exit = false, first = true;
      DiceGroup output = input, parsedGroup;
      Match match;
      
      while(matches.Count > 0 &&
            (exit == false || output is DiceSpecification))
      {
        match = matches.Dequeue();
        
#if DEBUG
      Console.WriteLine("Raw group: {0}", match.Value);
#endif
        
        // Parse the match as a dice group
        parsedGroup = parseDiceGroupFromMatch(match, out enter, out exit);
        
        if(!first && !parsedGroup.HasOperator && !exit)
        {
          throw new FormatException("Missing operator, likely an invalid " +
                                    "dice specification");
        }
        
        if(first &&
           output is DiceSpecification &&
           parsedGroup.OperatorIsMultiplication)
        {
          throw new FormatException("Dice string must not begin with " +
                                    "multiplication or division");
        }
        
        /* If we have passed an open-bracket (and not a close-bracket) then
         * we need to call this method recursively on the new group we just
         * created.
         */
        if(enter)
        {
          parsedGroup = parseGroupMatches(matches, parsedGroup);
        }
        
        // Add that newly found dice group to the output (as a child)
        output.Groups.Add(parsedGroup);
        first = false;
      }
      
      return output;
    }
    
    /* Uses a regex match instance to parse and return a dice group.
     */
    private static DiceGroup parseDiceGroupFromMatch(Match input,
                                                     out bool enter,
                                                     out bool exit)
    {
      DiceGroup output = new DiceGroup();
      enter = false;
      exit = false;
      
      // The stuff we need to extract from the regex match:
      GroupOperator? precedingOperator = null;
      bool openingParenthesis = false, closingParenthesis = false;
      int? numberOfDice = null, sidesPerDie = null, staticNumber = null;
      string operatorString;
      
      /* There are up to 8 group matches in this match:
       * 1     the operator            (always present)
       * 2     opening bracket         (not always present)
       * 3 & 4 IGNORE                  (these are the matched dice roll)
       * 5     number of dice          (not always present)
       * 6     sides per die           (not always present)
       * 7     static number           (not always present)
       * 8     closing bracket         (not always present)
       * 
       * The way I want to check is to look for 1, then 2 and 8, then try for 7
       * If 7 is not found then use 5 and 6 instead.
       */
      
      // Parse the operator
      operatorString = input.Groups[1].Value;
      if(operatorString.Length > 0)
      {
        switch(operatorString.Substring(0, 1))
        {
        case "+":
          precedingOperator = GroupOperator.Add;
          break;
        case "-":
          precedingOperator = GroupOperator.Subtract;
          break;
        case "*":
          precedingOperator = GroupOperator.Multiply;
          break;
        case "x":
          precedingOperator = GroupOperator.Multiply;
          break;
        case "/":
          precedingOperator = GroupOperator.Divide;
          break;
        }
      }
      
      // Parse any parentheses in this group
      if(input.Groups[2].Success)
      {
        openingParenthesis = true;
      }
      if(input.Groups[8].Success)
      {
        closingParenthesis = true;
      }
      
      // Parse either a static number or a dice roll
      if(input.Groups[7].Success)
      {
        staticNumber = Convert.ToInt32(input.Groups[7].Value);
      }
      else if(input.Groups[6].Success)
      {
        if(input.Groups[5].Success &&
           input.Groups[5].Value != String.Empty)
        {
          numberOfDice = Convert.ToInt32(input.Groups[5].Value);
        }
        else
        {
          numberOfDice = 1;
        }
        if(input.Groups[6].Value.Substring(0, 1) == "%")
        {
          sidesPerDie = 100;
        }
        else
        {
          sidesPerDie = Convert.ToInt32(input.Groups[6].Value);
        }
      }
      
      if(precedingOperator.HasValue)
      {
        output.Operator = precedingOperator.Value;
      }
      
      if(staticNumber.HasValue)
      {
        output.NumberOfDice = staticNumber;
        output.SidesPerDie = null;
      }
      else if(numberOfDice.HasValue && sidesPerDie.HasValue)
      {
        output.NumberOfDice = numberOfDice;
        output.SidesPerDie = sidesPerDie;
      }
      
      enter = (openingParenthesis && !closingParenthesis);
      exit = (closingParenthesis && !openingParenthesis);
      
      return output;
    }
    
    /* This method just strips whitespace from the dice specification as well
     * as parsing the number of rolls.  It returns a new dice specification
     * object, as well as modifying the original specification string, ready for
     * parsing the dice groups.
     */
    private static DiceSpecification parseSpecBasics(ref string diceSpec)
    {
      string processedDiceSpec;
      Regex matchNumberOfRolls;
      Match numRolls;
      DiceSpecification output = new DiceSpecification();
      
      // Firstly, remove all whitespace from the spec
      processedDiceSpec = Regex.Replace(diceSpec, MATCH_WHITESPACE, "");
      
      // Parse the number of rolls, and just leave the dice groups
      matchNumberOfRolls = new Regex(MATCH_NUMBER_OF_ROLLS);
      numRolls = matchNumberOfRolls.Match(processedDiceSpec);
      if(numRolls.Success)
      {
        output.NumberOfRolls =
          Convert.ToInt32(numRolls.Groups[1].Captures[0].Value);
      }
      processedDiceSpec = matchNumberOfRolls.Replace(processedDiceSpec, "");
      
      diceSpec = processedDiceSpec;
      return output;
    }
#endregion
  }
}
