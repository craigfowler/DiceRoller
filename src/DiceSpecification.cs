/*
 * DiceSpecification Created on 20/03/2009 by Craig Fowler
 * Copyright Craig Fowler
 */

using System;
using System.Text.RegularExpressions;

namespace CraigFowler.Diceroller
{
  internal sealed class DiceSpecification : DiceGroup
  {
#region constants
    private const string
      MATCH_WHITESPACE = @"\s",
      MATCH_NUMBER_OF_ROLLS = @"^(\d+)#",
      MATCH_DICE_GROUP = @"([-+*x/]*)(\()?(((\d*)[Dd]([%\d]+))|(\d+))(\))?";
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
    
    protected override string generateString(DiceGroupDisplay options)
    {
      throw new NotImplementedException();
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
      return parseDiceSpec(diceSpec);
    }
    
    private static DiceSpecification parseDiceSpec(string diceSpec)
    {
      /* Parse the basics of the dice spec (strip whitespace and get the number
       * of rolls
       */
      DiceSpecification output;
      DiceGroup parsedGroup;
      MatchCollection groupMatch;
      Regex matchDiceGroups = new Regex(MATCH_DICE_GROUP);
      output = parseSpecBasics(ref diceSpec);
      
#if DEBUG
      Console.WriteLine("Spec: '{0}'", diceSpec);
#endif
      
      /* Now, the dice specification is ready for parsing groups.
       * Use a regex to match the dice groups in the specification, then parse
       * each of them, using the groups in tht regex to extract the relevant
       * parts.
       */
      groupMatch = matchDiceGroups.Matches(diceSpec);
      foreach(Match group in groupMatch)
      {
        parsedGroup = parseGroup(group);
        // TODO: Still to do is to add the parsed dice group to the output
      }
      
      // throw new NotImplementedException();
      return output;
    }
    
    /* Uses a regex match instance to parse and return a dice group.
     */
    private static DiceGroup parseGroup(Match input)
    {
      DiceGroup output = new DiceGroup();
      
      // The stuff we need to extract from the regex match:
      GroupOperator? precedingOperator = null;
      bool openingParenthesis = false, closingParenthesis = false;
      int? numberOfDice = null, sidesPerDie = null, staticNumber = null;
      
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
      switch(input.Groups[1].Value)
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
      else
      {
        if(input.Groups[5].Success)
        {
          numberOfDice = Convert.ToInt32(input.Groups[5].Value);
        }
        else
        {
          numberOfDice = 1;
        }
        sidesPerDie = Convert.ToInt32(input.Groups[6].Value);
      }
      
      // TODO:  This needs refactoring so that it can be called recursively
      
#if DEBUG
      for(int i = 1; i < input.Groups.Count; i++)
      {
        if(input.Groups[i].Success)
        {
          Console.WriteLine("Group: '{0}',  RegexGroup {1}: '{2}'",
                            input.Value,
                            i,
                            input.Groups[i].Value);
        }
      }
#endif
      
      throw new NotImplementedException();
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
