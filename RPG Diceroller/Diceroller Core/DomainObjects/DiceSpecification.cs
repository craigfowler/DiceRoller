
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace CraigFowler.Gaming.Diceroller.DomainObjects
{
  public class DiceSpecification
  {
    #region constants
    
    private const string
      WHITESPACE_REGEX  = @"\s",
      NUM_ROLLS_REGEX   = @"^(\d+)#",
      DICEGROUP_REGEX   = @"([-+*x/]*)((\()|(((\d*)[Dd]([%\d]+))|(\d+))|(\)))";      
    
    private const string
      INVALID_NUMBER_OF_ROLLS_ERROR   = "Number of rolls must be one or more.",
      MISSING_OPERATOR_ERROR          = "Missing operator whilst parsing.",
      MULTIPLICATION_AT_START_ERROR   = "Dice string must not begin with " +
                                        "multiplication or division.",
      INVALID_DICE_SPEC_STRING        = "[Invalid dice specification]";
    
    private const int
      DEFAULT_NUMBER_OF_ROLLS   = 1;
    
    /* Indices of regex groups that we were interested in for parsing dice
     * groups
     */
    private const int
      OPERATOR_REGEX_GROUP      = 1,
      OPEN_BRACKET_REGEX_GROUP  = 3,
      NUMBER_DICE_REGEX_GROUP   = 6,
      NUMBER_SIDES_REGEX_GROUP  = 7,
      STATIC_NUMBER_REGEX_GROUP = 8,
      CLOSE_BRACKET_REGEX_GROUP = 9;
      
    #endregion
    
    #region fields
    private string specificationString;
    private DiceGroup parsedGroup;
    int numberOfRolls;
    decimal?
      rerollLowerThan,
      rerollHigherThan,
      explodeIfHigherThan,
      rollAgainIfHigherThan;
    #endregion
    
    #region properties
    
    //// <value>
    /// Gets or sets the dice specification
    /// </value>
    /// <exception cref="FormatException">
    /// If the value is invalid (and cannot be parsed into a valid
    /// specification) then this exception is thrown.
    /// </exception>
    public string SpecificationString
    {
      get {
        return specificationString;
      }
      set {
        specificationString = value;
        parsedGroup = null;
      }
    }
    
    //// <value>
    /// Gets or sets the number of independent times that this dice
    /// specification will be rolled.  Each roll produces an independent result.
    /// </value>
    public int NumberOfRolls
    {
      get {
        return numberOfRolls;
      }
      set {
          numberOfRolls = (value >= 0)? value : 0;
      }
    }
    
    public Nullable<decimal> ExplodingThreshold
    {
      get {
        return explodeIfHigherThan;
      }
      set {
        explodeIfHigherThan = value;
      }
    }
    
    public Nullable<decimal> RerollIfHigherThan
    {
      get {
        return rerollHigherThan;
      }
      set {
        rerollHigherThan = value;
      }
    }
    
    public Nullable<decimal> RerollIfLowerThan
    {
      get {
        return rerollLowerThan;
      }
      set {
        rerollLowerThan = value;
      }
    }
    
    public Nullable<decimal> RollAgainThreshold
    {
      get {
        return rollAgainIfHigherThan;
      }
      set {
        rollAgainIfHigherThan = value;
      }
    }
    
    #endregion
    
    #region privateMethods
    
    /// <summary>
    /// Parses a dice specification string and returns a dice group object
    /// that was created using the specification.
    /// </summary>
    /// <param name="spec">
    /// A <see cref="System.String"/> - the specification to parse.
    /// </param>
    /// <returns>
    /// A <see cref="DiceGroup"/> - the dice group created using that
    /// specification.
    /// </returns>
    /// <exception cref="FormatException">
    /// The 'downstream' method
    /// <see cref="parseGroup(ref Queue<Match>,ref DiceGroup,bool)"/> could
    /// throw this exception if the dice format has problems.
    /// </exception>
    private DiceGroup parseSpecification(string spec)
    {
      DiceGroup output;
      Queue<Match> groupMatches;
      Regex dicegroupRegex;
      
      stripWhitespace(ref spec);
      this.NumberOfRolls = parseNumberOfRolls(ref spec);
      
      dicegroupRegex = new Regex(DICEGROUP_REGEX,
                                 RegexOptions.Compiled);
      groupMatches = new Queue<Match>();
      
      foreach(Match group in dicegroupRegex.Matches(spec))
      {
        if(group.Value != String.Empty)
        {
          groupMatches.Enqueue(group);
        }
      }
      
      output = new DiceGroup();
      parseGroup(ref groupMatches, ref output, true);
      
      return output;
    }
    
    /// <summary>
    /// Configures and sets up a dice group, including the additiong of
    /// child/inner dice groups if applicable.
    /// </summary>
    /// <param name="matches">
    /// A <see cref="Queue"/> of
    /// <see cref="System.Text.RegularExpressions.Match"/> objects, representing
    /// the dice groups found in a dice specification string.
    /// </param>
    /// <param name="group">
    /// A <see cref="DiceGroup"/> object, the group to be configured.
    /// </param>
    /// <param name="topLevel">
    /// A <see cref="System.Boolean"/> that serves as a simple flag as to
    /// whether this is the 'top level' group or not.
    /// </param>
    /// <exception cref="FormatException">
    /// <para>
    /// If a dice group is missing an operator and is not the first 'inner
    /// group' within a parent group then this exception is thrown.  If it is
    /// the first 'inner group' then the <see cref="DiceGroupOperator"/> 'Add'
    /// is assumed.
    /// </para>
    /// <para>
    /// This exception can also be thrown if the first operator in the
    /// dice group structure is multiplication or division, as this is an
    /// impossible case.
    /// </para>
    /// </exception>
    private void parseGroup(ref Queue<Match> matches,
                            ref DiceGroup group,
                            bool topLevel)
    {
      bool exit = false, enter, first = true;
      int numDice, numSides;
      DiceGroupOperator? oper;
      DiceGroup parsedGroup;
      
      if(matches.Count > 1)
      {
        while(matches.Count > 0 && (exit == false || topLevel == true))
        {
          parseDiceGroupFromMatch(matches.Dequeue(),
                                  out oper,
                                  out numDice,
                                  out numSides,
                                  out enter,
                                  out exit);
          
          if(!exit)
          {
            /* Eliminate two possible error cases that make parsing impossible
             * and also set the dice group operator to 'Add' if this is the
             * first dice group in the spec (and no operator was specified)
             */
            if(!first && !oper.HasValue)
            {
              throw new FormatException(MISSING_OPERATOR_ERROR);
            }
            else if(topLevel && oper.HasValue &&
                    (
                     oper.Value == DiceGroupOperator.Multiply ||
                     oper.Value == DiceGroupOperator.Divide
                    ))
            {
              throw new FormatException(MULTIPLICATION_AT_START_ERROR);
            }
            else if(first && !oper.HasValue)
            {
              oper = DiceGroupOperator.Add;
            }
            
            // Create the dice group object to hold our newly-parsed group
            parsedGroup = new DiceGroup();
            parsedGroup.Operator = oper.Value;
            
            if(enter && !exit)
            {
              parseGroup(ref matches, ref parsedGroup, false);
            }
            else
            {
              if(numDice > 0)
              {
                parsedGroup.Dice = numDice;
              }
              if(numSides > 0)
              {
                parsedGroup.Sides = numSides;
              }
            }
            
            // Add the newly parsed group to the group we are working on
            group.Groups.Add(parsedGroup);
          }
          
          first = false;
        }
      }
      else
      {
        parseDiceGroupFromMatch(matches.Dequeue(),
                                out oper,
                                out numDice,
                                out numSides,
                                out enter,
                                out exit);
        
        // Handle some possible error conditions
        if(topLevel && oper.HasValue &&
           (
            oper.Value == DiceGroupOperator.Multiply ||
            oper.Value == DiceGroupOperator.Divide
           ))
        {
          throw new FormatException(MULTIPLICATION_AT_START_ERROR);
        }
        else if(!oper.HasValue)
        {
          oper = DiceGroupOperator.Add;
        }
        
        group.Operator = oper.Value;
        if(numDice > 0)
        {
          group.Dice = numDice;
        }
        if(numSides > 0)
        {
          group.Sides = numSides;
        }
      
      }
    }
    
    /// <summary>
    /// Parses the 'meat' of a regex dice group match and etracts the relevant
    /// dice group information from it.
    /// </summary>
    /// <param name="match">
    /// A <see cref="Match"/> representing the matched fragment of a dice
    /// specification string.
    /// </param>
    /// <param name="oper">
    /// A nullable <see cref="DiceGroupOperator"/> representing the operator
    /// found for this group (if any).
    /// </param>
    /// <param name="numDice">
    /// A <see cref="System.Int32"/> - the number of dice for this group.
    /// </param>
    /// <param name="numSides">
    /// A <see cref="System.Int32"/> - the number of sides per die.
    /// </param>
    /// <param name="enter">
    /// A <see cref="System.Boolean"/> - if this dice group includes an opening
    /// parenthesis then further dice groups found are 'inner groups'.  If this
    /// is the case then this will mark true.
    /// </param>
    /// <param name="exit">
    /// A <see cref="System.Boolean"/> - if this dice group includes a closing
    /// parenthesis then any further matches are no longer 'inner groups'.  In
    /// this case this will mark true.
    /// </param>
    private void parseDiceGroupFromMatch(Match match,
                                         out DiceGroupOperator? oper,
                                         out int numDice,
                                         out int numSides,
                                         out bool enter,
                                         out bool exit)
    {
      string operatorString;
      bool closeParen, openParen;
      
      oper = null;
      numDice = 0;
      numSides = 0;
      enter = false;
      exit = false;
      
      // Check for the operator
      operatorString = match.Groups[OPERATOR_REGEX_GROUP].Value;
      switch(operatorString)
      {
      case "+":
        oper = DiceGroupOperator.Add;
        break;
      case "-":
        oper = DiceGroupOperator.Subtract;
        break;
      case "/":
        oper = DiceGroupOperator.Divide;
        break;
      case "*":
        oper = DiceGroupOperator.Multiply;
        break;
      case "x":
        oper = DiceGroupOperator.Multiply;
        break;
      default:
        oper = null;
        break;
      }
      
      // Check for any parentheses
      openParen = match.Groups[OPEN_BRACKET_REGEX_GROUP].Success;
      closeParen = match.Groups[CLOSE_BRACKET_REGEX_GROUP].Success;
      
      // Parse the number of dice and number of sides
      if(match.Groups[STATIC_NUMBER_REGEX_GROUP].Success)
      {
        numDice = Int32.Parse(match.Groups[STATIC_NUMBER_REGEX_GROUP].Value);
        numSides = 1;
      }
      else if(match.Groups[NUMBER_SIDES_REGEX_GROUP].Success)
      {
        if(!String.IsNullOrEmpty(match.Groups[NUMBER_DICE_REGEX_GROUP].Value))
        {
          numDice = Int32.Parse(match.Groups[NUMBER_DICE_REGEX_GROUP].Value);
        }
        else
        {
          numDice = 1;
        }
        if(match.Groups[NUMBER_SIDES_REGEX_GROUP].Value.Substring(0, 1) == "%")
        {
          numSides = 100;
        }
        else
        {
          numSides = Int32.Parse(match.Groups[NUMBER_SIDES_REGEX_GROUP].Value);
        }
      }
      
      // Determine whether or not we are entering/exiting a dice group
      enter = (openParen && !closeParen);
      exit = (closeParen && !openParen);
    }
    
    /// <summary>
    /// Pares the number of rolls for a dice specification and removes the
    /// 'number of rolls' information from the dice specification.
    /// </summary>
    /// <param name="spec">
    /// A <see cref="System.String"/> - the dice specification string.  This
    /// will be modified if a number of rolls is specified.  Specifically the
    /// number of rolls information will be stripped.
    /// </param>
    /// <returns>
    /// A <see cref="System.Int32"/> - the number of dice rolls, defaults to 1
    /// if this information was not present in the spec.
    /// </returns>
    private int parseNumberOfRolls(ref string spec)
    {
      int output;
      Regex numberOfRollsRegex;
      Match matchNumberOfRolls;
      
      numberOfRollsRegex = new Regex(NUM_ROLLS_REGEX, RegexOptions.Compiled);
      matchNumberOfRolls = numberOfRollsRegex.Match(spec);
      
      if(matchNumberOfRolls.Success)
      {
        output = Int32.Parse(matchNumberOfRolls.Groups[1].Captures[0].Value);
        spec = numberOfRollsRegex.Replace(spec, String.Empty);
      }
      else
      {
        output = DEFAULT_NUMBER_OF_ROLLS;
      }
      
      return output;
    }
    
    /// <summary>
    /// Quite simply strips whitespace from the input string.
    /// </summary>
    /// <param name="spec">
    /// A <see cref="System.String"/> - the input string to have all whitepace
    /// stripped.  The string is modified in the process.
    /// </param>
    private void stripWhitespace(ref string spec)
    {
      Regex whitespaceRegex;
      
      whitespaceRegex = new Regex(WHITESPACE_REGEX, RegexOptions.Compiled);
      spec = whitespaceRegex.Replace(spec, String.Empty);
    }
    #endregion
    
    #region publicMethods
    public override string ToString()
    {
      string dgString, rollsString;
      DiceGroup dice;
      
      try
      {
        dice = GetDice();
      }
      catch(FormatException)
      {
        return INVALID_DICE_SPEC_STRING;
      }
      
      rollsString = (NumberOfRolls != 1)?
        String.Format("{0}#", NumberOfRolls) : String.Empty;
      
      dgString = (dice != null)? dice.ToString() : String.Empty;
      if(dgString.Length > 0 && dgString[0] == '+')
      {
        dgString = dgString.Substring(1);
      }
      
      return (!String.IsNullOrEmpty(dgString))?
        String.Format("{0}{1}", rollsString, dgString) : String.Empty;
    }
    
    public DiceGroup GetDice()
    {
      if(parsedGroup == null)
      {
        parsedGroup = parseSpecification(SpecificationString);
      }
      return parsedGroup;
    }
    
    public decimal RollOnce()
    {
      DiceGroup group = GetDice();
      return group.GetValue();
    }
    
    public decimal RollOnce(CalculationMethod method)
    {
      DiceGroup group = GetDice();
      return group.GetValue(method);
    }
    
    public decimal[] Roll()
    {
      decimal[] output = new decimal[NumberOfRolls];
      DiceGroup group = GetDice();
      
      for(int i = 0; i < output.Length; i++)
      {
        output[i] = group.GetValue();
      }
      
      return output;
    }
    
    public decimal[] Roll(CalculationMethod method)
    {
      decimal[] output = new decimal[NumberOfRolls];
      DiceGroup group = GetDice();
      
      for(int i = 0; i < output.Length; i++)
      {
        output[i] = group.GetValue(method);
      }
      
      return output;
    }
    #endregion
    
    #region staticMethods
    public static DiceGroup GetDice(string spec)
    {
      DiceSpecification diceSpec;
      diceSpec = new DiceSpecification(spec);
      return diceSpec.GetDice();
    }
    
    public static decimal RollOnce(string spec)
    {
      DiceSpecification diceSpec;
      diceSpec = new DiceSpecification(spec);
      return diceSpec.RollOnce();
    }
    
    public static decimal[] Roll(string spec)
    {
      DiceSpecification diceSpec;
      diceSpec = new DiceSpecification(spec);
      return diceSpec.Roll();
    }
    #endregion
    
    #region constructors
    public DiceSpecification()
    {
      specificationString = null;
      parsedGroup = null;
      numberOfRolls = DEFAULT_NUMBER_OF_ROLLS;
      rerollHigherThan = null;
      rerollLowerThan = null;
      explodeIfHigherThan = null;
      rollAgainIfHigherThan = null;
    }
    
    public DiceSpecification(string spec) : this()
    {
      this.SpecificationString = spec;
    }
    #endregion
  }
}
